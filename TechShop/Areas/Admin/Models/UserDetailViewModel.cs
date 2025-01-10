using TechShop.Models;

namespace TechShop.Areas.Admin.Models
{
    public class UserDetailViewModel
    {
        public User User { get; set; }
        public IEnumerable<Order> Invoices { get; set; }
    }
}
