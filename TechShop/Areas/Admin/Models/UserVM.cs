using Microsoft.Identity.Client;
using TechShop.Models;

namespace TechShop.Areas.Admin.Models
{
    public class UserVM
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Order> Order { get; set; }

    }
}
