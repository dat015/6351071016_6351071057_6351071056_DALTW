using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechShop.Areas.Customer.ViewModel;
using TechShop.Data;
using TechShop.Helper;
using TechShop.Models;
using TechShop.Services.Mail;
using TechShop.Utility;

namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private UserVM _userVM;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ApplicationDbContext db, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _db = db;
            _webHostEnvironment = webHostEnvironment;


        }

        private int? UserId
        {
            get
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdString, out int userId))
                {
                    Console.WriteLine("Converted user ID successfully.");
                    return userId;
                }
                Console.WriteLine("Failed to convert user ID.");
                return null;
            }
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.Error = "Vui lòng đăng nhập để tiếp tục!";
                return View("Error");
            }

            var userId = UserId;
            if (userId == null)
            {
                ViewBag.Error = "Đã xảy ra lỗi. Không lấy được ID người dùng.";
                return View("Error");
            }

            var user = await _db.User.FindAsync(userId.Value);
            if (user == null)
            {
                ViewBag.Error = "Đã xảy ra lỗi. Người dùng không tồn tại.";
                return View("Error");
            }

      

            _userVM = new UserVM
            {
                user = new Lazy<User>(() => user),
            };

            return View(_userVM);
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordVM model)
        {
            

            // Kiểm tra UserId
            if (UserId == null)
            {
                TempData["Error"] = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Index");
            }

            // Kiểm tra dữ liệu nhập
            if (model == null || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                TempData["Error"] = "Dữ liệu nhập không hợp lệ.";
                return RedirectToAction("Index");
            }

            if (model.ConfirmPassword != model.NewPassword)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp.";
                return RedirectToAction("Index");
            }

            try
            {
                // Tìm người dùng
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == (int)UserId);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy người dùng.";
                    return RedirectToAction("Index");
                }

                var crrPass = model.CurrentPassword.ToMd5Hash(user.RandomKey);
                if(crrPass != user.Password)
                {
                    TempData["Error"] = "Mật khẩu không khớp mật khẩu hiện tại.";
                    return RedirectToAction("Index");
                }

                // Đổi mật khẩu
                user.Password = model.NewPassword.ToMd5Hash(user.RandomKey);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Đổi mật khẩu thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangePasswordByOTP(ChangePasswordVM model)
        {


            if (model == null || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                TempData["Error"] = "Không tìm thấy dữ liệu nhập";
                return View("InputNewPassword");
            }

            if (model.ConfirmPassword != model.NewPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận phải giống mật khẩu nhập vào";

                return View("InputNewPassword");
            }

            try
            {
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == (int)UserId);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy người dùng";
                    return View("InputNewPassword");
                }

                user.Password = model.NewPassword.ToMd5Hash(user.RandomKey);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Đổi mật khẩu thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string UserName, string Email, string PhoneNumber, IFormFile ProfileImage)
        {
            if (ModelState.IsValid)
            {
                var userId = UserId;
                if (userId == null)
                {
                    ViewBag.Error = "Không thể lấy ID người dùng. Vui lòng đăng nhập lại.";
                    return View("Error");
                }

                var user = await _db.User.FindAsync(userId);
                if (user == null)
                {
                    ViewBag.Error = "Đã xảy ra lỗi. Người dùng không tồn tại.";
                    return View("Error");
                }

                // Update user information
                user.Name = UserName;
                user.PhoneNumber = PhoneNumber;

                // Handle image upload
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    var uniqueFileName = await UploadImage(user, ProfileImage);
                    user.Image = uniqueFileName;
                }

                _db.User.Update(user);
                await _db.SaveChangesAsync();
                _userVM ??= new UserVM();
                _userVM.user = new Lazy<User>(() => user);
                TempData["Success"] = "Thông tin của bạn đã được cập nhật thành công.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Đã xảy ra lỗi khi cập nhật thông tin của bạn. Vui lòng thử lại.";
            return View("Index", _userVM);
        }

        private async Task<string> UploadImage(User user, IFormFile profileImage)
        {
            // Ensure the uploads folder is within the web root
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Save the image to the path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
            }

            // Return the relative path
            return "/img/" + uniqueFileName;
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

        [HttpGet]
        public async Task<IActionResult> SendOTP()
        {
            var user = await _db.User.FirstOrDefaultAsync(p => p.Id == (int)UserId);
            if (user == null)
            {
                ViewBag.Error = "Người dùng không tồn tại!";
                return RedirectToAction("InputOTP");
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
                return RedirectToAction("InputOTP");
            }
            ViewBag.Email = user.Email;
            return RedirectToAction("InputOTP");
        }
        public  bool ValidateOTP( string otpCode)
        {
            var user =  _db.User.FirstOrDefault(p => p.Id == (int)UserId);

            // Tìm OTP theo email và mã OTP
            var otp = _db.OTPs.AsNoTracking().FirstOrDefault(o => o.Email == user.Email && o.OtpCode == otpCode);

            // Kiểm tra OTP hợp lệ và chưa hết hạn
            if (otp != null && otp.ExpirationTime >= DateTime.Now)
            {
                return true;
            }

            return false;
        }
        [HttpPost]
        public IActionResult VerifyOTP( string otpCode)
        {
            if ( ValidateOTP(otpCode))
            {
                ViewBag.Success = "Xác thực OTP thành công!";
                return View("InputNewPassword");
            }
            else
            {
                ViewBag.Error = "Mã OTP không hợp lệ hoặc đã hết hạn.";
                return RedirectToAction("InputOTP");
            }
        }

        public IActionResult InputOTP()
        {
            return View();
        }
    }
}
