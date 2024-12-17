using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Helper;
using TechShop.Models;
using TechShop.Repository;

namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var lstProduct = await _db.Products.OrderBy(p => p.ProductId).Include(cat => cat.CategoryOfProducts).Include(brd => brd.BrandOfProducts).ToListAsync();
            return View(lstProduct);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            ViewBag.Brands = new SelectList(_db.Brands, "BrandId", "BrandName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.Brands = new SelectList(_db.Brands, "BrandId", "BrandName", product.BrandId);
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var err in value.Errors)
                    {
                        errors.Add(err.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            else
            {
                var existingProduct = _db.Products.FirstOrDefault(p => p.ProductName == product.ProductName);

                if (existingProduct != null)
                {
                    // If the product name already exists, set an error message and return to the same view
                    TempData["error"] = "Sản phẩm đã tồn tại !";
                    return RedirectToAction("Create");
                }

                if (product.ImageUpLoad != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imgName = Guid.NewGuid().ToString() + "_" + product.ImageUpLoad.FileName;
                    string filePath = Path.Combine(uploadDir, imgName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpLoad.CopyToAsync(fs);
                    fs.Close();
                    product.Img = imgName;
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                TempData["success"] = "Tạo thành công sản phẩm";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Product product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            ViewBag.Brands = new SelectList(_db.Brands, "BrandId", "BrandName", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            ViewBag.Brands = new SelectList(_db.Brands, "BrandId", "BrandName", product.BrandId);

            var existed_product = await _db.Products.FindAsync(product.ProductId); // Lấy ra sản phẩm cũ

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model chưa hợp lệ";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string lstErrorMessage = string.Join('\n', errors);
                return BadRequest(lstErrorMessage);
            }
            else
            {
                if (product.ImageUpLoad != null)
                {
                    string upLoadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpLoad.FileName;
                    string filePathImage = Path.Combine(upLoadDir, imageName);
                    try
                    {
                        string oldImage = Path.Combine(upLoadDir, existed_product.Img);
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi xóa ảnh sản phẩm");
                    }

                    FileStream stream = new FileStream(filePathImage, FileMode.Create);
                    await product.ImageUpLoad.CopyToAsync(stream);
                    stream.Close();
                    existed_product.Img = imageName;
                }

                // update other product properties
                existed_product.ProductId = product.ProductId;
                existed_product.ProductName = product.ProductName;
                existed_product.BrandId = product.BrandId;
                existed_product.Description = product.Description;
                existed_product.PriceBase = product.PriceBase;

                _db.Products.Update(existed_product);
                await _db.SaveChangesAsync();
                TempData["success"]="Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId.Equals(id));
            if (!string.Equals(product.Img, "noname.jpg"))
            {
                try
                {
                    string root = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string pathImg = Path.Combine(root, product.Img);
                    if (System.IO.File.Exists(pathImg))
                    {
                        System.IO.File.Delete(pathImg);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi xóa ảnh sản phẩm");
                }
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["success"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index");
        }
    }
}
