using TechShop.Models;

namespace TechShop.ViewModel
{
    public class OrderVM
    {
        public Order order {  get; set; }
        public IEnumerable<Order> orders { get; set; }

        public IEnumerable<OrderDetail> orderDetails { get; set; }
    }
}
