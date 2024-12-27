using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using TechShop.Data;
using TechShop.Helper;
using TechShop.Models;
using TechShop.Utility;
using TechShop.ViewModel;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace TechShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Customer)]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;



        public OrderController(ApplicationDbContext db)
        {
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
        public async Task<ActionResult> Index(int cartId)
        {
            var listCart = await _db.CartDetails
                .Where(p => p.CartId == cartId && p.status == false)
                .Include(p => p.Specs)
                .ToListAsync();

            var listSpecsId = listCart.Select(p => p.Specs.Id).ToList();

            var listSpecs = await _db.specs
                .Where(s => listSpecsId.Contains(s.Id))
                .Include(s => s.Product)
                .ToListAsync();

            // Cấu hình JsonSerializerOptions với ReferenceHandler.Preserve để xử lý vòng lặp tham chiếu
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 64
            };

            var serializedCartDetails = JsonSerializer.Serialize(listCart, options);
            HttpContext.Session.SetString("CartDetails", serializedCartDetails);


            var checkouVM = new CheckoutVM()
            {
                specs = listSpecs,
                productOrder = listCart,
                paymentMethods = await _db.PaymentMethods.ToListAsync()
            };

            return View(checkouVM);
        }

        [HttpPost]
        public async Task<ActionResult> Index(CheckoutVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var serializedCartDetails = HttpContext.Session.GetString("CartDetails");
            if (string.IsNullOrEmpty(serializedCartDetails))
            {
                // Handle the case where session data is null or empty
                return View("Error");
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 64
            };
            var cartDetails = JsonSerializer.Deserialize<List<CartDetail>>(serializedCartDetails, options);

            if (cartDetails == null)
            {
                return RedirectToAction("Error");
            }

            var newOrder = new Order()
            {
                UserId = (int)userId,  // Kiểm tra kiểu của userId, cần ép kiểu đúng
                StatusPayment = SD.PaymentStatusPending,  // Kiểm tra giá trị SD.PaymentStatusPending có hợp lệ không
                StatusShipping = "Await",  // Nếu không có giá trị thì có thể để null, nhưng bạn đã gán chuỗi "Await"
                isAccept = false,  // Cái này đã có giá trị mặc định là false
                TotalAmount = cartDetails.Sum(pr => pr.price * pr.quantity),  // Kiểm tra nếu cartDetails không null
                OrderDate = DateTime.Now,  // Đảm bảo gán đúng ngày giờ hiện tại
                PaymentMethodId = model.PaymentMethodID,  // Kiểm tra PaymentMethodID từ model
                district = model.District,  // Kiểm tra model.District
                provice = model.Province,  // Kiểm tra model.Province
                ward = model.Ward,  // Kiểm tra model.Ward
                address = model.Address,  // Kiểm tra model.Address
                phoneNumber = model.Phone,  // Kiểm tra model.Phone
                Name = model.FirstName + " " + model.LastName,  // Kết hợp tên và họ
                Note = model.Note ?? "",  // Nếu Note không có giá trị, có thể để chuỗi rỗng hoặc null
                PaymentDate = DateTime.Now  // Nếu bạn cần gán PaymentDate, có thể gán như vậy
            };

            try
            {
                _db.Orders.Add(newOrder);
                await _db.SaveChangesAsync();

                if (!(await AddDetailCartToDetailOrder(cartDetails, newOrder.OrderId)))
                {
                    return RedirectToAction("Error");
                }
                return RedirectToAction("Success");
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);

                return RedirectToAction("Error");

            }


        }
        public async Task<bool> AddDetailCartToDetailOrder(List<CartDetail> cartDetails, int orderID)
        {
            if (cartDetails == null) return false;

            var orderDetails = new List<OrderDetail>();
            foreach (var item in cartDetails)
            {
                var orderDetail = new OrderDetail()
                {

                    specId = item.specId,
                    OrderId = orderID,
                    Quantity = item.quantity,
                    UnitPrice = item.price * item.quantity,
                };

                orderDetails.Add(orderDetail);
            }

            try
            {
                await _db.DetailsOrders.AddRangeAsync(orderDetails);
                await _db.SaveChangesAsync();

                if(!(await ChangeStatusCartDetails(cartDetails)))
                {
                    return false;

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeStatusCartDetails(List<CartDetail> cartDetails)
        {
            if (cartDetails == null || !cartDetails.Any())
                return false;
            var listId = cartDetails.Select(x => x.Id).ToList();
            var orderDetailsExisting = await _db.CartDetails.Where(o =>listId.Contains(o.Id)).ToListAsync();


            if (!orderDetailsExisting.Any())
                return false;

           
            foreach (var detail in orderDetailsExisting)
            {
                detail.status = true; 
            }

            await _db.SaveChangesAsync();
            return true;
        }


    }
}
