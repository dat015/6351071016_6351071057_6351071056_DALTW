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

       

        public async Task<IActionResult> LoadPartial(string partialName)
        {
            var orders = await _db.Orders.ToListAsync();
            switch (partialName)
            {
                case "TatCa":
                    return PartialView("_TatCaPartial",orders);
                case "ChoXacNhan":
                    orders = orders.Where(o => o.isAccept == false && o.StatusShipping == "0").ToList();
                    return PartialView("_ChoXacNhanPartial",orders);
                case "DangGiao":
                    orders = orders.Where(o => o.isAccept == true && o.StatusShipping == "1").ToList();
                    return PartialView("_DangGiaoPartial",orders);
                case "DaGiao":
                    orders = orders.Where(o => o.StatusShipping == "2" && o.isAccept == true).ToList();
                    return PartialView("_DaGiaoPartial",orders);
                default:
                    return PartialView("_TatCaPartial",orders);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOrder(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.isAccept = true;
            order.StatusShipping = "1";
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var order = await _db.Orders
                .Include(o => o.user)
                .Include(o => o.PaymentMethod)
                .Include(o => o.orderDetails)
                    .ThenInclude(od => od.Specs)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

    }
}
