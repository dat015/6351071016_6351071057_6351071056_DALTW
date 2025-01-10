using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Areas.Admin.Models;
using TechShop.Data;
using TechShop.Helper;
using TechShop.Models;
using TechShop.Utility;

namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<UserVM> getUserVM()
        {
            UserVM userVM = new UserVM()
            {
                Users = await _db.User.Include(u => u.Role).Where(u => u.Role.roleName == SD.Role_Customer).ToListAsync(),
                Order = await _db.Orders.Include(o => o.user).ToListAsync()
            };
            return userVM;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await getUserVM();
                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<User>());
            }

        }

        public ActionResult GetUserDetails(int id)
        {
            var user = _db.User.Find(id);
            var invoices = _db.Orders.Where(i => i.UserId == id && i.StatusShipping == "2").ToList();
            var model = new UserDetailViewModel
            {
                User = user,
                Invoices = invoices
            };
            return PartialView("_UserDetail", model);
        }

        public ActionResult GetInvoiceDetails(int id)
        {
            var invoice = _db.Orders.Find(id);
            var invoiceDetails = _db.DetailsOrders.Where(d => d.OrderId == id)
                .Include(d => d.Specs)
                .ThenInclude(d => d.Product)
                .ToList();
            var model = new InvoiceDetailViewModel
            {
                Invoice = invoice,
                InvoiceDetails = invoiceDetails
            };
            return PartialView("_InvoiceDetail", model);
        }
        [HttpPost]
        public async Task<JsonResult> LockUserAsync(int userId)
        {
            try
            {
                // Truy xuất người dùng từ cơ sở dữ liệu bất đồng bộ
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.IsLock = true;
                    await _db.SaveChangesAsync(); // Lưu thay đổi bất đồng bộ
                    return Json(new { success = true, message = "Khóa người dùng thành công." });
                }
                return Json(new { success = false, message = "Không tìm thấy người dùng." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> UnlockUserAsync(int userId)
        {
            try
            {
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.IsLock = false;
                    await _db.SaveChangesAsync(); // Lưu thay đổi bất đồng bộ
                    return Json(new { success = true, message = "Mở khóa người dùng thành công." });
                }
                return Json(new { success = false, message = "Không tìm thấy người dùng." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
