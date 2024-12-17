using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Helper;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModel;
using Microsoft.EntityFrameworkCore;



namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OrderVM> GetOrder()
        {
            
            return new OrderVM()
            {
                orders = await _db.Orders.ToListAsync()
            };
        }

        public async Task<IActionResult> Index()
        {
            var orderVM = await GetOrder();
            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int OrderId)
        {
            try
            {
                Console.WriteLine("ok");
                var order = await _db.Orders.FirstOrDefaultAsync(o => o.OrderId == OrderId);

                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng cần xóa." });
                }

                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();

                var orders = await GetOrder();
                return Json(new { success = true, orders = orders.orders });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string statusPayment, string statusShipping, string Note)
        {
            if(statusPayment == "" || statusShipping == "" || Note == "")
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!"});
            }
            try
            {
                var order = await _db.Orders.FindAsync(id);
                if (order == null) { 
                    return Json(new { success = false,  message = "Không tìm thấy đơn hàng cần cập nhật." });
                }

                order.StatusPayment = statusPayment;
                order.StatusShipping = statusShipping;
                order.Note = Note;

                await _db.SaveChangesAsync();

                var orders = await GetOrder();
                return Json(new { success = true, orders = orders.orders });
            }
            catch (Exception ex) {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật.", error = ex.Message });
            }
        }



    }
}
