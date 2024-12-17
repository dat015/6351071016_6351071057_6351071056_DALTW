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


    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> lstCategory = await _db.Categories.ToListAsync();
            return View(lstCategory);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Giá trị chưa hợp lệ. Kiểm tra lại.";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach(var err in value.Errors)
                    {
                        errors.Add(err.ErrorMessage);
                    }
                }
                string lstError = string.Join(", ", errors);
                return BadRequest(lstError);
            }
            else
            {
                if (_db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    TempData["error"] = "Danh mục đã tồn tại";
                    return View();
                }
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            Category category = await _db.Categories.Where(i => i.CategoryId == id).FirstOrDefaultAsync();
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category obj)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Giá trị chưa hợp lệ.Kiểm tra lại";
                return View(obj);
            }
            _db.Categories.Update(obj);
            await _db.SaveChangesAsync();
            TempData["success"]="Sửa danh mục thành công.";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            Category obj = await _db.Categories.FindAsync(id);
            _db.Categories.Remove(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Xóa danh mục thành công.";
            return RedirectToAction("Index");
        }
    }
}
