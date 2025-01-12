using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TechShop.Areas.Admin.Models;
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
        public async Task<ProductVM> GetProductVM()
        {
            return new ProductVM()
            {
                Specs = new Lazy<Task<List<Specs>>>(() => _db.specs
                                                             .Include(p => p.Product)
                                                             .ToListAsync()),
                Products = new Lazy<Task<List<Product>>>(() => _db.Products
                                                                    .Include(Brand => Brand.BrandOfProducts)
                                                                    .Include(Category => Category.CategoryOfProducts)
                                                                    .Where(p => p.IsHide == false)
                                                                    .ToListAsync()),
                Cate = new Lazy<Task<List<Category>>>(() => _db.Categories.ToListAsync()),
                CPUs = new Lazy<Task<List<CPU>>>(() => _db.CPUs.ToListAsync()),
                ManHinhs = new Lazy<Task<List<ManHinh>>>(() => _db.ManHinhs.ToListAsync()),
                ODia = new Lazy<Task<List<ODia>>>(() => _db.ODias.ToListAsync()),
                CardDoHoas = new Lazy<Task<List<CardDoHoa>>>(() => _db.CardDoHoa.ToListAsync()),
                Pins = new Lazy<Task<List<Pin>>>(() => _db.Pins.ToListAsync()),
                RAMs = new Lazy<Task<List<RAM>>>(() => _db.RAMs.ToListAsync()),
                Brands = new Lazy<Task<List<Brand>>>(() => _db.Brands.ToListAsync()),
                trongLuongs = new Lazy<Task<List<TrongLuong>>>(() => _db.TrongLuongs.ToListAsync()),
            };
        }
        public async Task<IActionResult> Index()
        {
            var productVM = await GetProductVM();
            return View(productVM);
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
                    try
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }
                        string imgName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(product.ImageUpLoad.FileName);
                        string filePath = Path.Combine(uploadDir, imgName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageUpLoad.CopyToAsync(fs);
                        }
                        product.Img = imgName;
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "Lỗi khi lưu ảnh: " + ex.Message;
                        return RedirectToAction("Create");
                    }
                }

                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                TempData["success"] = "Tạo thành công sản phẩm";
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> GetSpecsByProduct(int id)
        {
            try
            {
                var productVm = await GetProductVM();
                productVm.ProductId = id;

                // Lọc cấu hình của sản phẩm theo ID
                var filteredSpecs = await _db.specs
                     .Include(s => s.Product)
                     .Include(s => s.Config)
                         .ThenInclude(c => c.CPU)
                     .Include(s => s.Config)
                         .ThenInclude(c => c.RAM)
                     .Include(s => s.Config)
                         .ThenInclude(c => c.CardDoHoa)
                     .Include(s => s.Config)
                         .ThenInclude(c => c.ODia)
                     .Include(s => s.Config)
                         .ThenInclude(c => c.ManHinh)
                     .Include(s => s.Config)
                         .ThenInclude(c => c.Pin)
                     .Include(s => s.Config)
                         .ThenInclude(c => c.TrongLuong)
                     .Where(spec => spec.ProductId == id)
                     .ToListAsync(); // Trả về List thay vì Lazy
                // Gán giá trị mới cho productVm
                productVm.Specs = new Lazy<Task<List<Specs>>>(() => Task.FromResult(filteredSpecs));

                var reviews = _db.Reviews
                                 .Where(r => r.ProductId == id)
                                 .Include(r => r.User)
                                 .ToList();
                productVm.reviews = new Lazy<Task<List<Review>>>(() => Task.FromResult(reviews));

                // Trả về Partial View với thông tin mới
                return PartialView("PartialProduct", productVm);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                var review = _db.Reviews.Find(id);
                if (review == null)
                {
                    return Json(new { success = false, message = "Đánh giá không tồn tại." });
                }

                _db.Reviews.Remove(review);
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa đánh giá." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSpec(int id, int cpuId, int ramId, int newCartDoHoa, int oDiaId, int manHinhId, int pinId)
        {
            // Kiểm tra tính hợp lệ của các tham số
            if (id <= 0 || cpuId <= 0 || ramId <= 0 || oDiaId <= 0 || manHinhId <= 0 || pinId <= 0)
            {
                return BadRequest("Các tham số không hợp lệ");
            }

            // Lấy thông tin cấu hình hiện tại
            var existingSpec = await _db.specs
                .Include(s => s.Config)
                .Include(s => s.Product)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (existingSpec == null)
            {
                return BadRequest("Không tìm thấy cấu hình cần sửa");
            }

            var config = await _db.cauHinhs.FindAsync(existingSpec.ConfigId);
            if (config == null)
            {
                return BadRequest("Không tìm thấy cấu hình cần sửa");
            }

            // Lấy thông tin các thành phần cấu hình mới từ cơ sở dữ liệu
            var cpu = await _db.CPUs.FindAsync(cpuId);
            var ram = await _db.RAMs.FindAsync(ramId);
            var cardDoHoa = await _db.CardDoHoa.FindAsync(newCartDoHoa);
            var oDia = await _db.ODias.FindAsync(oDiaId);
            var manHinh = await _db.ManHinhs.FindAsync(manHinhId);
            var pin = await _db.Pins.FindAsync(pinId);

            if (cpu == null || ram == null || cardDoHoa == null || oDia == null || manHinh == null || pin == null)
            {
                return BadRequest("Không tìm thấy một hoặc nhiều thành phần cấu hình tương ứng");
            }

            // Cập nhật các thuộc tính cấu hình với các giá trị mới
            config.CPUId = cpuId;
            config.RAMId = ramId;
            config.ODiaId = oDiaId;
            config.ManHinhId = manHinhId;
            config.PinId = pinId;
            config.CardDoHoaId = newCartDoHoa;

            try
            {
                // Tính lại giá mới cho cấu hình
                //existingSpec.Price = cpu.AdditionalPrice + ram.AdditionalPrice
                //                    + cardDoHoa.AdditionalPrice + oDia.AdditionalPrice
                //                    + manHinh.AdditionalPrice + pin.AdditionalPrice
                //                    + existingSpec.Product.PriceBase;

                // Cập nhật lại cấu hình và lưu vào cơ sở dữ liệu
                await _db.SaveChangesAsync();
                existingSpec.ConfigId = config.MaCauHinh;
                await _db.SaveChangesAsync();

                // Trả về thông tin cấu hình đã cập nhật
                var updatedConfig = new
                {
                    Price = existingSpec.Price,  // Giá đã được tính toán
                    CPU = cpu.TenCPU,
                    RAM = ram.DungLuong,
                    CartDoHoa = cardDoHoa.TenCardDoHoa,
                    ODia = oDia.TenODia,
                    ManHinh = manHinh.KichThuocManHinh,
                    Pin = pin.DungLuongPin
                };

                return Ok(updatedConfig); // Trả về cấu hình đã cập nhật
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSpec(int specId)
        {
            if (specId <= 0)
            {
                return Json(new { Success = false, Message = "Không tìm thấy cấu hình đã chọn!" });
            }

            try
            {
                var spec = await _db.specs.FindAsync(specId);
                if (spec == null)
                {
                    return Json(new { Success = false, Message = "Không tìm thấy cấu hình đã chọn!" });

                }
                _db.specs.Remove(spec);
                await _db.SaveChangesAsync();
                return Json(new { Success = true, Message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });

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
                TempData["success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                // Xóa tất cả các bản ghi liên quan đến ProductId trong cauHinhs trước khi xóa sản phẩm
                //var relatedConfigs = _db.specs.Where(c => c.ProductId == id);
                //_db.specs.RemoveRange(relatedConfigs);
                //await _db.SaveChangesAsync();

                // Sau đó mới xóa sản phẩm
                var product = await _db.Products.FindAsync(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }
                product.IsHide = true;
                await _db.SaveChangesAsync();


                return Json(new { success = true, message = "Xóa sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Không thể xóa sản phẩm này! Vui lòng liên hệ bộ phận IT!" });

            }


        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(int productId, string description, string productName, decimal priceBase, IFormFile image, int categoryId, int brandId)
        {
            try
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                var product = await _db.Products.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false });
                }

                // Cập nhật các thuộc tính của sản phẩm
                product.ProductName = productName;
                product.PriceBase = priceBase;
                product.CategoryId = categoryId;
                product.Description = description;
                product.BrandId = brandId;

                if (image != null && image.Length > 0)
                {
                    var uniqueFileName = await UploadImage(product, image);
                    product.Img = uniqueFileName;
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _db.SaveChangesAsync();

                product = await _db.Products
                    .Include(s => s.BrandOfProducts)
                    .Include(s => s.CategoryOfProducts)
                    .FirstOrDefaultAsync(s => s.ProductId == productId);

                return Json(new
                {
                    success = true,
                    newImageUrl = product.Img,
                    newCategoryName = product.CategoryOfProducts?.CategoryName,
                    newBrandName = product.BrandOfProducts?.BrandName,
                    newDescription = product.Description
                });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
        private async Task<string> UploadImage(Product user, IFormFile profileImage)
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

        [HttpPost]
        public async Task<IActionResult> AddSpec(int productId, int cpuId, int ramId, int cardDoHoaId, int oDiaId, int manHinhId, int pinId, int trongLuongId)
        {
            try
            {
                // Kiểm tra sản phẩm
                var existingProduct = await _db.Products.FindAsync(productId);
                if (existingProduct == null)
                {
                    return NotFound("Sản phẩm không tồn tại.");
                }

                // Kiểm tra các thành phần cấu hình
                var existingCPU = await _db.CPUs.FindAsync(cpuId);
                if (existingCPU == null)
                {
                    return NotFound("CPU không tồn tại.");
                }

                var existingRAM = await _db.RAMs.FindAsync(ramId);
                if (existingRAM == null)
                {
                    return NotFound("RAM không tồn tại.");
                }

                var existingVGA = await _db.CardDoHoa.FindAsync(cardDoHoaId);
                if (existingVGA == null)
                {
                    return NotFound("Card đồ họa không tồn tại.");
                }

                var existingODia = await _db.ODias.FindAsync(oDiaId);
                if (existingODia == null)
                {
                    return NotFound("Ổ đĩa không tồn tại.");
                }

                var existingManHinh = await _db.ManHinhs.FindAsync(manHinhId);
                if (existingManHinh == null)
                {
                    return NotFound("Màn hình không tồn tại.");
                }

                var existingPin = await _db.Pins.FindAsync(pinId);
                if (existingPin == null)
                {
                    return NotFound("Pin không tồn tại.");
                }

                var existingTrongLuong = await _db.TrongLuongs.FindAsync(trongLuongId);
                if (existingTrongLuong == null)
                {
                    return NotFound("Trọng lượng không tồn tại.");
                }

                // Tạo cấu hình sản phẩm mới
                var newProductConfig = new CauHinh
                {
                    CPUId = cpuId,
                    RAMId = ramId,
                    CardDoHoaId = cardDoHoaId,
                    ODiaId = oDiaId,
                    ManHinhId = manHinhId,
                    PinId = pinId,
                    TrongLuongId = trongLuongId
                };

                _db.cauHinhs.Add(newProductConfig);
                await _db.SaveChangesAsync();

                // Thêm cấu hình vào specs
                await _db.Database.ExecuteSqlRawAsync(
         "INSERT INTO specs (ConfigId, Price, ProductId) VALUES ({0}, {1}, {2})",
         newProductConfig.MaCauHinh, 0, productId);


                return Ok(new
                {
                    message = "Cấu hình sản phẩm đã được thêm thành công.",
                    newSpec = await _db.specs
                         .Include(s => s.Product)
                         .Include(s => s.Config)
                             .ThenInclude(c => c.CPU)
                         .Include(s => s.Config)
                             .ThenInclude(c => c.RAM)
                         .Include(s => s.Config)
                             .ThenInclude(c => c.CardDoHoa)
                         .Include(s => s.Config)
                             .ThenInclude(c => c.ODia)
                         .Include(s => s.Config)
                             .ThenInclude(c => c.ManHinh)
                         .Include(s => s.Config)
                             .ThenInclude(c => c.Pin)
                         .Include(s => s.Config)
                             .ThenInclude(c => c.TrongLuong)
                         .Where(spec => spec.ProductId == productId && spec.ConfigId == newProductConfig.MaCauHinh)
                         .FirstOrDefaultAsync()
                });
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                return StatusCode(500, new { message = "Có lỗi xảy ra", error = ex.Message });
            }
        }



       


    }
}
