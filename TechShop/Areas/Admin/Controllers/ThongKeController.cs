using Microsoft.AspNetCore.Mvc;
using Spire.Doc;
using System.Diagnostics;
using TechShop.Data;
using TechShop.Areas.Admin.Models;
using ShoeWeb.Areas.Admin.Admin_ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Humanizer;

namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ThongKeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ThongKeController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> BestSellingProducts()
        {
            try
            {
                var orders = await _db.DetailsOrders
                    .Include(o => o.Specs.Product) // Bao gồm thông tin sản phẩm từ bảng liên kết
                    .GroupBy(o => o.Specs.ProductId) // Nhóm theo ProductId
                    .Select(g => new
                    {
                        ProductId = g.Key, // Lấy ProductId từ nhóm
                        ProductName = g.First().Specs.Product.ProductName, // Lấy tên sản phẩm
                        TotalSold = g.Sum(o => o.Quantity) // Tổng số lượng sản phẩm bán được
                    })
                    .OrderByDescending(g => g.TotalSold) // Sắp xếp theo số lượng bán giảm dần
                    .Take(12) // Lấy 12 sản phẩm bán nhiều nhất
                    .ToListAsync();

                return Json(new { values = orders });
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> TopRatedProducts()
        {
            try
            {
                var orders = await _db.DetailsOrders
                    .Include(o => o.Specs.Product) // Bao gồm thông tin sản phẩm từ bảng liên kết
                    .ThenInclude(p => p.reviews) // Bao gồm bảng reviews
                    .GroupBy(o => o.Specs.ProductId) // Nhóm theo ProductId
                    .Select(g => new
                    {
                        ProductId = g.Key, // Lấy ProductId từ nhóm
                        ProductName = g.First().Specs.Product.ProductName, // Lấy tên sản phẩm
                        TotalSold = g.Sum(o => o.Quantity), // Tổng số lượng bán được
                        AverageRating = g.First().Specs.Product.reviews.Any()
                            ? g.First().Specs.Product.reviews.Average(r => r.Rating) // Tính số sao trung bình
                            : 0, // Nếu không có review, đặt mặc định là 0
                        TotalReviews = g.First().Specs.Product.reviews.Count() // Tổng số lượt đánh giá
                    })
                    .OrderByDescending(g => g.AverageRating) // Sắp xếp theo số sao trung bình giảm dần
                    .ThenByDescending(g => g.TotalReviews) // Tiếp tục sắp xếp theo số lượt đánh giá giảm dần
                    .Take(12) // Lấy 12 sản phẩm bán nhiều nhất
                    .ToListAsync();



                return Json(new { values = orders });
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult LoadPartial(string type)
        {
            switch (type)
            {
                case "revenue":
                    // Trả về PartialView thống kê doanh thu
                    return PartialView("DoanhThu");
                case "best-selling":
                    // Trả về PartialView sản phẩm bán chạy
                    return PartialView("SanPhamBanChay");
                case "top-rated":
                    // Trả về PartialView sản phẩm đánh giá tốt
                    return PartialView("DanhGiaTot");
                default:
                    // Nếu không xác định, trả về một thông báo lỗi hoặc PartialView mặc định
                    return PartialView("DoanhThu");
            }
        }
        [HttpGet]
        public async Task<ActionResult> Statistic_TheoVung()
        {
            try
            {
                int currentYear = DateTime.Now.Year;

                // Lấy dữ liệu của năm nay
                var monthData = await _db.Orders
                    .Where(p => p.StatusShipping == "2" && p.OrderDate.Year == currentYear)
                    .ToListAsync();

                // Nhóm dữ liệu theo tháng và tính tổng số tiền theo mỗi tháng
                var orders = monthData
                    .GroupBy(o => o.OrderDate.Month)
                    .Select(g => new StatisticVM
                    {
                        TotalAmount = g.Sum(o => o.TotalAmount), // Xử lý null
                        CreatedDate = new DateTime(currentYear, g.Key, 1) // Mốc ngày đầu tháng
                    })
                    .ToList();

                // Đảm bảo có đầy đủ 12 tháng
                for (int i = 1; i <= 12; i++)
                {
                    if (!orders.Any(o => o.CreatedDate.Month == i))
                    {
                        orders.Add(new StatisticVM
                        {
                            CreatedDate = new DateTime(currentYear, i, 1),
                            TotalAmount = 0 // Thêm tháng chưa có dữ liệu với số tiền bằng 0
                        });
                    }
                }

                // Sắp xếp theo tháng
                var data = orders
                    .OrderBy(o => o.CreatedDate)
                    .Select(o => new
                    {
                        Date = o.CreatedDate.ToString("yyyy-MM"),
                        TotalAmount = o.TotalAmount
                    })
                    .ToList();

                return Json(new { values = data });
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Statistic_LuongDon()
        {
            try
            {
                List<StatisticVM> orders;
                int currentYear = DateTime.Now.Year; // Lấy năm hiện tại

                // Lấy dữ liệu của năm nay
                var monthData = await _db.Orders
                    .Where(o => o.OrderDate.Year == currentYear) // Lọc đơn hàng trong năm nay
                    .ToListAsync();

                // Nhóm dữ liệu theo tháng và tính tổng số lượng đơn hàng theo mỗi tháng
                orders = monthData.Where(p => p.StatusShipping == "2")
                    .GroupBy(o => o.OrderDate.Month)
                    .Select(g => new StatisticVM
                    {
                        // Thay vì tính tổng số tiền, ta tính tổng số lượng đơn hàng
                        TotalAmount = g.Count(), // Đếm số lượng đơn hàng trong tháng
                        CreatedDate = new DateTime(currentYear, g.Key, 1) // Mốc ngày đầu tháng
                    })
                    .OrderBy(o => o.CreatedDate) // Sắp xếp theo tháng
                    .ToList();

                // Đảm bảo có đầy đủ 12 tháng
                for (int i = 1; i <= 12; i++)
                {
                    if (!orders.Any(o => o.CreatedDate.Month == i))
                    {
                        orders.Add(new StatisticVM
                        {
                            CreatedDate = new DateTime(currentYear, i, 1),
                            TotalAmount = 0 // Thêm tháng chưa có dữ liệu với số lượng đơn bằng 0
                        });
                    }
                }

                // Trả về dữ liệu dưới dạng JSON
                var data = orders.OrderBy(o => o.CreatedDate).Select(o => new
                {
                    Date = o.CreatedDate.ToString("yyyy-MM"),
                    TotalAmount = o.TotalAmount // Sử dụng TotalAmount để chứa số lượng đơn hàng
                }).ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportRevenueToPDF(DateTime fromDate, DateTime toDate)
        {
            var orders = _db.Orders
                .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate  && o.StatusShipping == "2")
                .Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    o.TotalAmount,
                    CustomerName = o.Name,
                    CustomerPhone = o.phoneNumber,
                    Products = o.orderDetails.Select(od => new
                    {
                        Name = od.Specs.Product.ProductName,
                        Quantity = od.Quantity,
                        Price = od.Specs.Price,
                        UnitPrice = od.Quantity * od.Specs.Price
                    }).ToList()
                }).ToList();

            // Tổng doanh thu
            var totalRevenue = orders.Sum(o => o.TotalAmount);

            // Lấy đường dẫn gốc của dự án
            string wwwRootPath = _env.WebRootPath;

            // Tạo đường dẫn đầy đủ
            string pathTemplate = Path.Combine(wwwRootPath, "BaoCao", "Bao_Cao_Doanh_Thu_Template.docx");
            string pathExportDocx = Path.Combine(wwwRootPath, "BaoCao", "Bao_Cao_Doanh_Thu_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx");
            string pathExportPdf = Path.Combine(wwwRootPath, "BaoCao", "Bao_Cao_Doanh_Thu_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");

            // Mở template
            Document document = new Document();
            document.LoadFromFile(pathTemplate);

            // Thay thế các placeholder tổng quan
            document.Replace("{NgayLap}", DateTime.Now.ToString("dd/MM/yyyy"), false, true);
            document.Replace("{fromDate}", fromDate.ToString("dd/MM/yyyy"), false, true);
            document.Replace("{toDate}", toDate.ToString("dd/MM/yyyy"), false, true);
            document.Replace("{totalRevenue}", totalRevenue.ToString("N0"), false, true);
            document.Replace("{Quantity}", orders.Count().ToString(), false, true);

            // Tạo dữ liệu cho bảng sản phẩm
            var productDetails = orders.SelectMany(o => o.Products)
                                       .GroupBy(p => p.Name)
                                       .Select(g => new
                                       {
                                           ProductName = g.Key,
                                           ProductQuantity = g.Sum(p => p.Quantity),
                                           ProductPrice = g.First().Price,
                                           ProductUnitPrice = g.Sum(p => p.Quantity * p.UnitPrice)
                                       }).ToList();

            // Lấy bảng đầu tiên trong tài liệu
            Spire.Doc.Table table = (Spire.Doc.Table)document.Sections[0].Tables[0];

            // Lấy hàng mẫu (giả sử hàng mẫu là hàng đầu tiên trong bảng)
            Spire.Doc.TableRow templateRow = table.Rows[1];

            // Lặp qua từng sản phẩm và thêm dữ liệu vào bảng
            foreach (var item in productDetails)
            {
                // Nhân bản hàng mẫu
                Spire.Doc.TableRow newRow = templateRow.Clone() as Spire.Doc.TableRow;

                // Điền dữ liệu vào các ô
                newRow.Cells[0].Paragraphs[0].Text = item.ProductName;
                newRow.Cells[1].Paragraphs[0].Text = item.ProductQuantity.ToString();
                newRow.Cells[2].Paragraphs[0].Text = item.ProductPrice.ToString("N0");
                newRow.Cells[3].Paragraphs[0].Text = item.ProductUnitPrice.ToString("N0");

                // Thêm hàng mới vào bảng
                table.Rows.Add(newRow);
            }

            // Xóa hàng mẫu (nếu không cần hiển thị trong kết quả)
            table.Rows.Remove(templateRow);

            // Thay thế doanh thu theo kênh bán hàng
            string salesChannels = "Online\t" + totalRevenue.ToString("N0") + " VNĐ\t100%\n" + "Cửa hàng\t0 VNĐ\t0%";
            document.Replace("{salesChannels}", salesChannels, false, true);

            // Lưu file Word mới
            document.SaveToFile(pathExportDocx, FileFormat.Docx);

            // Chuyển đổi file Word sang PDF
            Spire.Doc.Document wordDoc = new Spire.Doc.Document();
            wordDoc.LoadFromFile(pathExportDocx);
            wordDoc.SaveToFile(pathExportPdf, Spire.Doc.FileFormat.PDF);

            // Trả file PDF về trình duyệt để tải xuống
            var fileBytes = await System.IO.File.ReadAllBytesAsync(pathExportPdf);
            var fileName = Path.GetFileName(pathExportPdf);
            return File(fileBytes, "application/pdf", fileName);
        }

    }
}
