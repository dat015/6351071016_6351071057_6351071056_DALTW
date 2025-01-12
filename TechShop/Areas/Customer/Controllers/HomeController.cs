using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechShop.Models;
using TechShop.ViewModel;
using TechShop.Data;
using Microsoft.EntityFrameworkCore;
using TechShop.Helper;
using Microsoft.AspNetCore.Authorization;
using TechShop.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TechShop.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
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
        public async Task<IActionResult> Index()
        {
            List<Category> list_cate = await _db.Categories.ToListAsync();
            List<Product> list_product = await _db.Products
                .Include(p => p.Specifications)
                .Where(p => p.IsHide == false)
                .ToListAsync();
            List<Role> roles = await _db.Roles.ToListAsync();
            foreach (var item in roles)
            {
                Console.WriteLine(item.roleName);
            }
            var viewModel = new HomeViewModel(list_cate, list_product);

            return View(viewModel);
        }

        public IActionResult ProductByCategory(int idCate)
        {
            List<Product> list_product = _db.Products.Where(x => x.CategoryId == idCate && x.IsHide == false).OrderBy(x => x.ProductName).ToList();
            return View(list_product);
        }

        [HttpGet]
        public async Task<IActionResult> Product(
       string? searchTerm,
       int? cateId,
       int? ramId,
       string? sortOrder,
       int? OdiaId,
       int? manHinhId,
       int? VGAId,
       int? CPUId,
       int? brandId,
       decimal? fromPrice,
       decimal? toPrice
   )
        {
            // Lấy danh sách sản phẩm
            var products = _db.Products
                .Where(p => p.IsHide == false)
                .AsQueryable();
            var specs = _db.specs
                .Include(p => p.Product)
                .Include(p => p.Config)
                .AsQueryable();

            // Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm));
                specs = specs.Where(s => s.Product.ProductName.Contains(searchTerm));
            }

            if (fromPrice.HasValue && fromPrice.HasValue)
            {
                specs = specs.Where(s => s.Price >= fromPrice && s.Price <= toPrice);

            }

            if (brandId.HasValue)
            {
                products = products.Where(p => p.BrandId == brandId.Value);
                specs = specs.Where(s => s.Product.BrandId == brandId.Value);

            }

            // Lọc theo danh mục
            if (cateId.HasValue)
            {
                products = products.Where(p => p.CategoryId == cateId.Value);
                specs = specs.Where(p => p.Product.CategoryId == cateId.Value);

            }

            // Lọc theo RAM
            if (ramId.HasValue)
            {
                products = products.Where(p => p.CauHinhProducts.Any(ch => ch.RAMId == ramId.Value));
                specs = specs.Where(p => p.Config.RAMId == ramId.Value);

            }

            // Lọc theo ổ đĩa (ODiaId)
            if (OdiaId.HasValue)
            {
                products = products.Where(p => p.CauHinhProducts.Any(ch => ch.ODiaId == OdiaId.Value));
                specs = specs.Where(p => p.Config.ODiaId == OdiaId.Value);

            }

            // Lọc theo màn hình (manHinhId)
            if (manHinhId.HasValue)
            {
                products = products.Where(p => p.CauHinhProducts.Any(ch => ch.ManHinhId == manHinhId.Value));
                specs = specs.Where(p => p.Config.ManHinhId == manHinhId.Value);

            }

            // Lọc theo card đồ họa (VGAId)
            if (VGAId.HasValue)
            {
                products = products.Where(p => p.CauHinhProducts.Any(ch => ch.CardDoHoaId == VGAId.Value));
                specs = specs.Where(p => p.Config.CardDoHoaId == VGAId.Value);

            }

            // Lọc theo CPU (CPUId)
            if (CPUId.HasValue)
            {
                products = products.Where(p => p.CauHinhProducts.Any(ch => ch.CPUId == CPUId.Value));
                specs = specs.Where(p => p.Config.CPUId == CPUId.Value);

            }

            // Sắp xếp sản phẩm
            switch (sortOrder)
            {
                case "PriceBaseAsc":
                    products = products.OrderBy(p => p.PriceBase);
                    specs = specs.OrderBy(p => p.Price);
                    break;
                case "PriceBaseDesc":
                    products = products.OrderByDescending(p => p.PriceBase);
                    specs = specs.OrderByDescending(p => p.Price);

                    break;
                case "nameAsc":
                    products = products.OrderBy(p => p.ProductName);
                    specs = specs.OrderBy(p => p.Product.ProductName);
                    break;
                case "nameDesc":
                    products = products.OrderByDescending(p => p.ProductName);
                    specs = specs.OrderByDescending(p => p.Product.ProductName);
                    break;
            }

            // Chuyển thành danh sách
            var productList = await products.ToListAsync();

            // Lấy danh sách specs
            var specsList = await specs.ToListAsync();

            // Lấy các thông tin khác cho ViewModel
            var productVM = new ProductViewModel()
            {
                Products = productList,
                Cate = cateId.HasValue ? await _db.Categories.FindAsync(cateId) : null,
                RAMs = await _db.RAMs.ToListAsync(),
                CPUs = await _db.CPUs.ToListAsync(),
                ManHinhs = await _db.ManHinhs.ToListAsync(),
                ODia = await _db.ODias.ToListAsync(),
                cardDoHoas = await _db.CardDoHoa.ToListAsync(),
                Brands = await _db.Brands.ToListAsync(),
                specs = specsList ?? new List<Specs>() // Gắn danh sách specs
            };

            return View(productVM);
        }



        [HttpGet]
        public async Task<IActionResult> ProductDetail(int? id, int? configId)
        {
            Specs productByConfig = await _db.specs.Where(pr => pr.ProductId == id && pr.ConfigId == configId)
                 .Include(pr => pr.Product)
                 .Include(pr => pr.Config)
                 .FirstOrDefaultAsync();

            CauHinh config = await _db.cauHinhs.Where(cf => cf.MaCauHinh == configId)
                .Include(cf => cf.RAM)
                .Include(cf => cf.CPU)
                .Include(cf => cf.ODia)
                .Include(cf => cf.ManHinh)
                .Include(cf => cf.CardDoHoa)
                .Include(cf => cf.TrongLuong)
                .Include(cf => cf.Pin)
                .FirstOrDefaultAsync();

            if (productByConfig == null)
            {
                ViewData["Error"] = "Không tìm thấy sản phẩm!";
                return View("Error");
            }
            var products = await _db.Products.Where(pr => pr.CategoryId == productByConfig.Product.CategoryId).ToListAsync();
            var ConfigOfProduct = await _db.specs
            .Where(p => p.ProductId == productByConfig.ProductId)
            .Select(p => new ConfigDto
            {
                SpecId = p.Id,
                ProductId = p.ProductId,
                Product = p.Product,
                ConfigId = p.ConfigId,
                Price = p.Price,
                CPU = p.Config.CPU,
                RAM = p.Config.RAM,
                Pin = p.Config.Pin,
                ODia = p.Config.ODia,
                CardDoHoa = p.Config.CardDoHoa,
                TrongLuong = p.Config.TrongLuong,
                ManHinh = p.Config.ManHinh
            })
            .ToListAsync();


            var listReview = await _db.Reviews.Where(s => s.ProductId == productByConfig.ProductId)
                .Include(s => s.User)
                .ToListAsync();
            var rating = listReview.Where(s => s.Rating != null).ToList();
            var relatedProducts = await _db.Products.Where(p => p.CategoryId == productByConfig.Product.CategoryId && p.CategoryId != id)
                .Include(s => s.Specifications)
                .Take(4) 
                .ToListAsync();

            var detailProductVM = new DetailProductVM()
            {
                product = productByConfig,
                products = products,
                cauHinhs = ConfigOfProduct,
                config = config,
                Reviews = listReview,
                Rating = rating.Sum(s => s.Rating),
                ReviewCount = rating.Count(),
                RelatedProducts = relatedProducts
            };

            return View(detailProductVM);
        }

        [HttpPost]
        [Area("Customer")]

        public async Task<IActionResult> AddComment(int productId, string comment)
        {
            if (ModelState.IsValid)
            {
                var UserId = (int)userId;
                var user = await _db.User.FindAsync(UserId);

                var review = new Review
                {
                    ProductId = productId,
                    UserId = (int)userId,
                    Comment = comment,
                    ReviewDate = DateTime.Now
                };
                _db.Reviews.Add(review);
                await _db.SaveChangesAsync();

                // Trả về JSON
                return Json(new
                {
                    success = true,
                    comment = new
                    {
                        userName = user.Name,
                        userImage = user.Image,
                        content = comment,
                        date = review.ReviewDate.ToString("dd/MM/yyyy HH:mm")
                    }
                });
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
