using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.ViewModel;
using TechShop.Models;
using TechShop.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using TechShop.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using TechShop.Areas.Customer.ViewModel;
using TechShop.Services.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace TechShop.Areas.Customer.Controllers
{
    [Area("Customer")]




    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;

        }

        private int? userId
        {
            get
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdString, out int UserId))
                {
                    Console.WriteLine(" chuyen doi duoc id");

                    return UserId;
                }
                Console.WriteLine("Khong chuyen doi duoc id");
                return null;
            }
        }

        [HttpGet]
        public IActionResult Register()
        {   

            return View();
        }
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var role = _db.Roles.Where(r => r.roleName == SD.Role_Admin).FirstOrDefault(); // mac dinh la customer
                    model.role = role;
                    model.roleId = role.roleId;
                    Console.WriteLine(role.roleId + " " + role.roleName);
                    var customer = _mapper.Map<User>(model); // map model sang kieu User bang automapper
                    customer.RandomKey = MyUtil.GenerateRandomKey();

                    // mã hóa mật khấu => MD5
                    customer.Password = model.Password.ToMd5Hash(customer.RandomKey);

                    _db.Add(customer);
                    _db.SaveChanges();

                    // Tạo liên kết xác nhận email
                    var callbackUrl = Url.Action("ConfirmEmail", "User", new { area = "Customer", email = customer.Email }, protocol: HttpContext.Request.Scheme);
                    SendMail.SendEmail(model.Email, "Xác nhận tài khoản của bạn", $"Vui lòng nhấp vào <a href='{callbackUrl}'>đây</a> để xác nhận tài khoản.", "");

                    return View("EmailCheck");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    ModelState.AddModelError("", "An error occurred while registering. Please try again.");
                }
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Model Error: {error}");
                }
                return View(model);
            }
            return View(model);
        }
        public string GenerateOTPCode(int length = 6)
        {
            var random = new Random();
            string otp = string.Empty;

            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10).ToString(); // Tạo số ngẫu nhiên từ 0-9
            }

            return otp;
        }

        [HttpPost]
        public async Task<IActionResult> SendOTP(string email)
        {
            var user = await _db.User.FirstOrDefaultAsync(p => p.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Người dùng không tồn tại!";
                return View("InputEmail");
            }

            // Tạo mã OTP
            var otpCode = GenerateOTPCode();

            // Tạo đối tượng OTP
            var otp = new OTP()
            {
                UserId = user.Id,
                Email = user.Email,
                OtpCode = otpCode,
                ExpirationTime = DateTime.Now.AddMinutes(5), // OTP hết hạn sau 5 phút
            };

            // Lưu OTP vào cơ sở dữ liệu
            _db.OTPs.Add(otp);
            await _db.SaveChangesAsync();

            // Gửi OTP qua email
            try
            {
                SendMail.SendEmail(
                    user.Email,
                    "Mã OTP xác nhận",
                    $"Mã OTP của bạn là: <strong>{otpCode}</strong>. Mã này sẽ hết hạn sau 5 phút.",
                    ""
                );
                ViewBag.Success = "Mã OTP đã được gửi tới email của bạn!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi gửi email: {ex.Message}";
                return View("InputEmail");
            }
            ViewBag.Email = user.Email;
            return View("InputOTP");
        }
        public bool ValidateOTP(string email, string otpCode)
        {
            // Tìm OTP theo email và mã OTP
            var otp = _db.OTPs.AsNoTracking().FirstOrDefault(o => o.Email == email && o.OtpCode == otpCode);

            // Kiểm tra OTP hợp lệ và chưa hết hạn
            if (otp != null && otp.ExpirationTime >= DateTime.Now)
            {
                return true;
            }

            return false;
        }
        [HttpPost]
        public IActionResult VerifyOTP(string email, string otpCode)
        {
            if (ValidateOTP(email, otpCode))
            {
                ViewBag.Success = "Xác thực OTP thành công!";
                ViewBag.Email = email;
                return View("InputNewPassword");
            }
            else
            {
                ViewBag.Error = "Mã OTP không hợp lệ hoặc đã hết hạn.";
                return View("InputOTP");
            }
        }
        public async Task<ActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (model == null || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ViewBag.Error = "Không tìm thấy dữ liệu nhập";
                return View("InputNewPassword");
            }

            if (model.ConfirmPassword != model.NewPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận phải giống mật khẩu nhập vào";
                ViewBag.Email = model.Email;

                return View("InputNewPassword");
            }

            try
            {
                var user = await _db.User.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    ViewBag.Error = "Không tìm thấy người dùng";
                    ViewBag.Email = model.Email;
                    return View("InputNewPassword");
                }

                user.Password = model.NewPassword.ToMd5Hash(user.RandomKey);
                await _db.SaveChangesAsync();
                return View("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Email = model.Email;
                return View("InputNewPassword");
            }
        }
        public IActionResult InputEmail()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email)
        {


            // Tìm người dùng theo userId
            var user = await _db.User.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Cập nhật trạng thái xác nhận email
            user.IsEmailConfirmed = true;
            _db.Update(user);
            await _db.SaveChangesAsync();
            ViewBag.Success = "Xác nhận Email thành công! Bạn có thể đăng nhập để tiếp tục.";

            return View("Login");
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Email không hợp lệ!";
                return View("Login");
            }

            // Tìm người dùng theo email
            var user = await _db.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                // Tạo liên kết xác nhận email
                var callbackUrl = Url.Action("ConfirmEmail", "User", new { area = "Customer", email = user.Email }, protocol: HttpContext.Request.Scheme);

                // Gửi lại email xác nhận
                SendMail.SendEmail(user.Email, "Xác nhận tài khoản của bạn", $"Vui lòng nhấp vào <a href='{callbackUrl}'>đây</a> để xác nhận tài khoản.", "");

                ViewBag.Success = "Email xác nhận đã được gửi lại!";
            }
            else
            {
                ViewBag.ErrorMessage = "Không tìm thấy tài khoản với email này!";
            }

            // Trả về view đăng nhập với thông báo
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {

                //Kiem tra tai khoan
                var customer = _db.User.FirstOrDefault(customer => customer.Email == model.Email);
                if (customer == null)
                {
                    ModelState.AddModelError("Error", "Sai thông tin đăng nhập");
                }

                else
                {
                    if (customer.Password != model.Password.ToMd5Hash(customer.RandomKey))
                    {
                        ModelState.AddModelError("Error", "Sai thông tin đăng nhập");
                    }
                    else
                    {
                        if (customer.IsEmailConfirmed == false)
                        {
                            ViewBag.Error = "Tài khoản của bạn chưa được xác thực! Vui Lòng kiểm tra Email";
                            ViewBag.ResendMail = "yes";
                            return View(model);
                        }
                        var role = _db.Roles.Where(r => r.roleId == customer.RoleId).FirstOrDefault();
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, customer.Email),
                            new Claim(ClaimTypes.Name, customer.Name),
                            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                            new Claim(ClaimTypes.Role, role.roleName)
                        };
                        var claimIdentity = new ClaimsIdentity(claims, "ApplicationCookie");// danh tính người dùng qua 1 tập hợp các claim được liên kết với quá trình đăng nhập
                        var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal); // yêu cầu sử dụng xác thực cookie

                        if (role.roleName == SD.Role_Admin)
                        {
                            return Redirect("/Admin/Admin/Index");
                        }
                        else
                        {
                            return Redirect("/Customer/Home/Index");
                        }
                    }
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Model Error: {error}");
                }
                return View(model);
            }
            return View();
        }
    }
}
