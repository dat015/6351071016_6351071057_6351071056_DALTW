using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechShop.Models;
using TechShop.ViewModel;
using TechShop.Data;
using Microsoft.EntityFrameworkCore;
using TechShop.Helper;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<IActionResult> Index()
        {
            List<Category> list_cate = await _db.Categories.ToListAsync();
            List<Product> list_product = await _db.Products
                .Include(p => p.Specifications)
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
            List<Product> list_product = _db.Products.Where(x => x.CategoryId == idCate).OrderBy(x => x.ProductName).ToList();
            return View(list_product);
        }

        public async Task<IActionResult> Product(string sortOrder, int? cateId)
        {
            var products = _db.Products.AsQueryable();

            if (cateId.HasValue)
            {
                products = products.Where(p => p.CategoryId == cateId.Value);
            }

            switch (sortOrder)
            {
                case "PriceBaseAsc":
                    products = products.OrderBy(p => p.PriceBase);
                    break;
                case "PriceBaseDesc":
                    products = products.OrderByDescending(p => p.PriceBase);
                    break;
                case "nameAsc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                default:
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
            }

            List<Product> result = await products.ToListAsync();

            Category category = await _db.Categories.FindAsync(cateId);

            return View(new ProductViewModel(result, category));
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



            var detailProductVM = new DetailProductVM()
            {
                product = productByConfig,
                products = products,
                cauHinhs = ConfigOfProduct,
                config = config
            };

            return View(detailProductVM);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
