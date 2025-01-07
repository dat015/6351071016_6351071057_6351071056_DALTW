using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ConfigController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ConfigController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Action cho RAM
        public async Task<IActionResult> RAM()
        {
            List<RAM> listRAM;
            try
            {
                listRAM = await _db.RAMs.ToListAsync();
                return View(listRAM);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(null);
            }
        }

        // Action cho CPU
        public async Task<IActionResult> CPU()
        {
            List<CPU> listCPU;
            try
            {
                listCPU = await _db.CPUs.ToListAsync();
                return View(listCPU);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(null);
            }
        }

        // Action cho Card Đồ Họa
        public async Task<IActionResult> CardDoHoa()
        {
            List<CardDoHoa> listCardDoHoa;
            try
            {
                listCardDoHoa = await _db.CardDoHoa.ToListAsync();
                return View(listCardDoHoa);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(null);
            }
        }
        [HttpPost]
        public JsonResult DeleteCardDoHoa(int id)
        {
            try
            {
                var card = _db.CardDoHoa.Find(id);
                if (card != null)
                {
                    _db.CardDoHoa.Remove(card);
                    _db.SaveChanges();

                    return Json(new { success = true, message = "Xóa thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy card đồ họa." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateCardDoHoa(int id, string TenCardDoHoa, decimal AdditionalPrice)
        {
            try
            {
                var card = _db.CardDoHoa.Find(id);
                if (card != null)
                {
                    card.TenCardDoHoa = TenCardDoHoa;
                    card.AdditionalPrice = AdditionalPrice;

                    _db.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy card đồ họa." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCardDoHoa(string TenCardDoHoa, decimal AdditionalPrice)
        {
            try
            {
                // Thêm mới vào database
                var newCard = new CardDoHoa
                {
                    TenCardDoHoa = TenCardDoHoa,
                    AdditionalPrice = AdditionalPrice
                };

                _db.CardDoHoa.Add(newCard);
                await _db.SaveChangesAsync();

                // Trả về đối tượng vừa thêm với ID
                return Json(new
                {
                    success = true,
                    message = "Thêm mới thành công!",
                    data = new
                    {
                        CardDoHoaId = newCard.CardDoHoaId,
                        TenCardDoHoa = newCard.TenCardDoHoa,
                        AdditionalPrice = newCard.AdditionalPrice
                    } 
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Đã xảy ra lỗi: " + ex.Message
                });
            }
        }


        public async Task<IActionResult> ODia()
        {
            List<ODia> listODia;
            try
            {
                listODia = await _db.ODias.ToListAsync();
                return View(listODia);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(null);
            }
        }

        // Action cho Màn Hình
        public async Task<IActionResult> ManHinh()
        {
            List<ManHinh> listManHinh;
            try
            {
                listManHinh = await _db.ManHinhs.ToListAsync();
                return View(listManHinh);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(null);
            }
        }

        // Action cho Pin
        public async Task<IActionResult> Pin()
        {
            List<Pin> listPin;
            try
            {
                listPin = await _db.Pins.ToListAsync();
                return View(listPin);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(null);
            }
        }

        // Action cho Trọng Lượng
        public async Task<IActionResult> TrongLuong()
        {
            List<TrongLuong> listTrongLuong;
            try
            {
                listTrongLuong = await _db.TrongLuongs.ToListAsync();
                return View(listTrongLuong);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(null);
            }
        }
    }
}
