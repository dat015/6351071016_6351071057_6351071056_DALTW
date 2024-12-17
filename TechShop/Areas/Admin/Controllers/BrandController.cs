using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechShop.Data;
using TechShop.Helper;
using TechShop.Models;

namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]


    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BrandController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Brand> lstBrands = await _db.Brands.ToListAsync();
            return View(lstBrands);
        }
        
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                string lstError = string.Join("\n", errors);
                return BadRequest(lstError);
            }
            else
            {
                if (_db.Brands.Any(c => c.BrandName == brand.BrandName))
                {
                    TempData["error"] = "Nhãn hàng đã tồn tại";
                    return View();
                }
                _db.Brands.Add(brand);
                await _db.SaveChangesAsync();
                TempData["success"] = "Thêm nhãn hàng thành công.";
                return RedirectToAction("Index");
            }

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            Brand brand = await _db.Brands.Where(i => i.BrandId == id).FirstOrDefaultAsync();
            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Brand obj)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Giá trị chưa hợp lệ. Kiểm tra lại";
                return View(obj);
            }
            _db.Brands.Update(obj);
            await _db.SaveChangesAsync();
            TempData["success"]="Sửa nhãn hàng thành công.";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            Brand obj = await _db.Brands.FindAsync(id);
            _db.Brands.Remove(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Xóa nhãn hàng thành công.";
            return RedirectToAction("Index");
        }
    }
}
