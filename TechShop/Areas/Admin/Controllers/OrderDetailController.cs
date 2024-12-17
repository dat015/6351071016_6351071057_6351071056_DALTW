using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Helper;

namespace TechShop.Areas.Admin.Controllers
{
    public class OrderDetailController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
