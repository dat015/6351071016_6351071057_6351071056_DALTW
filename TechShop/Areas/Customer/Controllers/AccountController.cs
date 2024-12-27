using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using TechShop.Areas.Customer.ViewModel;
using TechShop.Data;
using TechShop.Models;
using TechShop.Utility;

namespace TechShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Customer)]
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

            var orderWaiting = await _db.Orders
                .Where(o => o.UserId == user.Id && !o.isAccept)
                .ToListAsync();

            var orderWaitingShip = await _db.Orders
                .Where(o => o.UserId == user.Id && o.isAccept && o.StatusShipping == "0")
                .ToListAsync();

            var orderShipping = await _db.Orders
                .Where(o => o.UserId == user.Id && o.isAccept && o.StatusShipping == "1")
                .ToListAsync();

            var orderSuccess = await _db.Orders
                .Where(o => o.UserId == user.Id && o.isAccept && o.StatusShipping == "2")
                .ToListAsync();

            _userVM = new UserVM
            {
                user = new Lazy<User>(() => user),
                orderWaiting = new Lazy<List<Order>>(() => orderWaiting),
                orderWaitingShip = new Lazy<List<Order>>(() => orderWaitingShip),
                orderShipping = new Lazy<List<Order>>(() => orderShipping),
                orderSuccess = new Lazy<List<Order>>(() => orderSuccess),
                orderDetail = new Lazy<List<OrderDetail>>(),
                order = new Lazy<Order>()
            };

            return View(_userVM);
        }

        [HttpGet]
        public async Task<IActionResult> DetailOrder(int orderId)
        {
            try
            {
                var order = await _db.Orders
                                     .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return NotFound();
                }

                var orderItems = await _db.DetailsOrders
                    .Where(od => od.OrderId == orderId)
                    .Include(od => od.Specs)
                    .ThenInclude(s => s.Product)
                    .ToListAsync();

                _userVM ??= new UserVM();
                _userVM.order = new Lazy<Order>(() => order);
                _userVM.orderDetail = new Lazy<List<OrderDetail>>(() => orderItems);

                return PartialView("_OrderDetails", _userVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching order details: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching order details.");
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteOrderItem(int orderItemId)
        {
            try
            {
                // Logic xóa order item
                var orderItem = await _db.DetailsOrders.FirstOrDefaultAsync(o => o.Id == orderItemId);
                if (orderItem != null)
                {
                    _db.DetailsOrders.Remove(orderItem);
                    await _db.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong đơn hàng." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteOrder(int orderId)
        {
            try
            {
                // Logic xóa order item
                var orderItem = await _db.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (orderItem != null)
                {
                    _db.Orders.Remove(orderItem);
                    await _db.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong đơn hàng." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SubmitRating(int productId, byte rating, string comment)
        {
            // Lưu đánh giá vào cơ sở dữ liệu
            var review = new Review
            {
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                ReviewDate = DateTime.Now,
                UserId = (int)UserId
            };
            try
            {
                _db.Reviews.Add(review);
                _db.SaveChanges();
                return Json(new { success = true, Message = "Đánh giá thành công!" });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json(new { success = false, Message = ex.Message });

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


    }
}