using TechShop.Models;

namespace TechShop.Areas.Admin.Models
{
    public class InvoiceDetailViewModel
    {
        public IEnumerable<OrderDetail> InvoiceDetails { get; set; }
        public Order Invoice { get; set; }
    }
}
