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

        [HttpPost]
        public async Task<JsonResult> AddRAM(string dungLuong, decimal additionalPrice)
        {
            var ram = new RAM
            {
                DungLuong = dungLuong,
                AdditionalPrice = additionalPrice
            };

            _db.RAMs.Add(ram);
            await _db.SaveChangesAsync();

            return Json(new { success = true, data = ram });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRAM(int id, string dungLuong, decimal additionalPrice)
        {
            var ram = await _db.RAMs.FindAsync(id);
            if (ram == null)
            {
                return Json(new { success = false, message = "RAM không tồn tại" });
            }

            ram.DungLuong = dungLuong;
            ram.AdditionalPrice = additionalPrice;

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRAM(int id)
        {
            var ram = await _db.RAMs.FindAsync(id);
            if (ram == null)
            {
                return Json(new { success = false, message = "RAM không tồn tại" });
            }

            _db.RAMs.Remove(ram);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
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

        [HttpPost]
        public JsonResult DeleteCPU(int id)
        {
            try
            {
                var cpu = _db.CPUs.Find(id);
                if (cpu != null)
                {
                    _db.CPUs.Remove(cpu);
                    _db.SaveChanges();

                    return Json(new { success = true, message = "Xóa CPU thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy CPU." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateCPU(int id, string TenCPU, decimal AdditionalPrice)
        {
            try
            {
                var cpu = _db.CPUs.Find(id);
                if (cpu != null)
                {
                    cpu.TenCPU = TenCPU;
                    cpu.AdditionalPrice = AdditionalPrice;

                    _db.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy CPU." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCPU(string TenCPU, decimal AdditionalPrice)
        {
            try
            {
                // Thêm mới CPU vào database
                var newCPU = new CPU
                {
                    TenCPU = TenCPU,
                    AdditionalPrice = AdditionalPrice
                };

                _db.CPUs.Add(newCPU);
                await _db.SaveChangesAsync();

                // Trả về đối tượng vừa thêm với ID
                return Json(new
                {
                    success = true,
                    message = "Thêm mới CPU thành công!",
                    data = new
                    {
                        CPUId = newCPU.CPUId,
                        TenCPU = newCPU.TenCPU,
                        AdditionalPrice = newCPU.AdditionalPrice
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

        [HttpPost]
        public async Task<JsonResult> DeleteODia(int id)
        {
            try
            {
                var oDia = await _db.ODias.FindAsync(id);
                if (oDia != null)
                {
                    _db.ODias.Remove(oDia);
                    await _db.SaveChangesAsync();

                    return Json(new { success = true, message = "Xóa ổ đĩa thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy ổ đĩa." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateODia(int id, string tenODia, decimal additionalPrice)
        {
            try
            {
                var oDia = await _db.ODias.FindAsync(id);
                if (oDia != null)
                {
                    oDia.TenODia = tenODia;
                    oDia.AdditionalPrice = additionalPrice;

                    await _db.SaveChangesAsync();

                    return Json(new { success = true, message = "Cập nhật ổ đĩa thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy ổ đĩa." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddODia(string tenODia, decimal additionalPrice)
        {
            try
            {
                var newODia = new ODia
                {
                    TenODia = tenODia,
                    AdditionalPrice = additionalPrice
                };

                _db.ODias.Add(newODia);
                await _db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Thêm mới ổ đĩa thành công!",
                    data = new
                    {
                        ODiaId = newODia.ODiaId,
                        TenODia = newODia.TenODia,
                        AdditionalPrice = newODia.AdditionalPrice
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
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

        [HttpPost]
        public async Task<JsonResult> DeleteManHinh(int id)
        {
            try
            {
                var manHinh = await _db.ManHinhs.FindAsync(id);
                if (manHinh != null)
                {
                    _db.ManHinhs.Remove(manHinh);
                    await _db.SaveChangesAsync();

                    return Json(new { success = true, message = "Xóa màn hình thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy màn hình." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateManHinh(int id, string kichThuocManHinh, decimal additionalPrice)
        {
            try
            {
                var manHinh = await _db.ManHinhs.FindAsync(id);
                if (manHinh != null)
                {
                    manHinh.KichThuocManHinh = kichThuocManHinh;
                    manHinh.AdditionalPrice = additionalPrice;

                    await _db.SaveChangesAsync();

                    return Json(new { success = true, message = "Cập nhật màn hình thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy màn hình." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddManHinh(string kichThuocManHinh, decimal additionalPrice)
        {
            try
            {
                var newManHinh = new ManHinh
                {
                    KichThuocManHinh = kichThuocManHinh,
                    AdditionalPrice = additionalPrice
                };

                _db.ManHinhs.Add(newManHinh);
                await _db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Thêm mới màn hình thành công!",
                    data = new
                    {
                        ManHinhId = newManHinh.ManHinhId,
                        KichThuocManHinh = newManHinh.KichThuocManHinh,
                        AdditionalPrice = newManHinh.AdditionalPrice
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
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

        [HttpPost]
        public JsonResult AddPin(string dungLuongPin, decimal additionalPrice)
        {
            if (string.IsNullOrEmpty(dungLuongPin) || additionalPrice <= 0)
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ!" });
            }

            var pin = new Pin
            {
                DungLuongPin = dungLuongPin,
                AdditionalPrice = additionalPrice
            };

            _db.Pins.Add(pin);
            _db.SaveChanges();

            return Json(new
            {
                success = true,
                data = new { pin.PinId, pin.DungLuongPin, pin.AdditionalPrice }
            });
        }

        // Cập nhật Pin (POST)
        [HttpPost]
        public JsonResult UpdatePin(int id, string dungLuongPin, decimal additionalPrice)
        {
            var pin = _db.Pins.FirstOrDefault(p => p.PinId == id);
            if (pin == null)
            {
                return Json(new { success = false, message = "Không tìm thấy pin!" });
            }

            pin.DungLuongPin = dungLuongPin;
            pin.AdditionalPrice = additionalPrice;
            _db.SaveChanges();

            return Json(new { success = true });
        }

        // Xóa Pin (POST)
        [HttpPost]
        public JsonResult DeletePin(int id)
        {
            var pin = _db.Pins.FirstOrDefault(p => p.PinId == id);
            if (pin == null)
            {
                return Json(new { success = false, message = "Không tìm thấy pin!" });
            }

            _db.Pins.Remove(pin);
            _db.SaveChanges();

            return Json(new { success = true });
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
        [HttpPost]
        public async Task<JsonResult> AddTrongLuong(string soKg)
        {
            if (string.IsNullOrWhiteSpace(soKg))
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ!" });
            }

            var trongLuong = new TrongLuong
            {
                SoKg = soKg
            };

            _db.TrongLuongs.Add(trongLuong);
            await _db.SaveChangesAsync();

            return Json(new { success = true, data = new { trongLuongId = trongLuong.TrongLuongId, soKg = trongLuong.SoKg } });
        }

        // POST: Admin/Config/UpdateTrongLuong
        [HttpPost]
        public async Task<JsonResult> UpdateTrongLuong(int id, string soKg)
        {
            var trongLuong = await _db.TrongLuongs.FindAsync(id);

            if (trongLuong == null)
            {
                return Json(new { success = false, message = "Không tìm thấy trọng lượng!" });
            }

            if (string.IsNullOrWhiteSpace(soKg))
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ!" });
            }

            trongLuong.SoKg = soKg;
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }

        // POST: Admin/Config/DeleteTrongLuong
        [HttpPost]
        public async Task<JsonResult> DeleteTrongLuong(int id)
        {
            var trongLuong = await _db.TrongLuongs.FindAsync(id);

            if (trongLuong == null)
            {
                return Json(new { success = false, message = "Không tìm thấy trọng lượng!" });
            }

            _db.TrongLuongs.Remove(trongLuong);
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
