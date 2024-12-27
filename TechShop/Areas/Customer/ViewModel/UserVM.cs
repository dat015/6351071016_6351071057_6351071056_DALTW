using TechShop.Models;

namespace TechShop.Areas.Customer.ViewModel
{
    public class UserVM
    {
        public Lazy<User> user { get; set; } = new Lazy<User>(() => new User());
        public Lazy<List<Order>> orderWaiting { get; set; } = new Lazy<List<Order>>(() => new List<Order>());
        public Lazy<List<Order>> orderWaitingShip { get; set; } = new Lazy<List<Order>>(() => new List<Order>());
        public Lazy<List<Order>> orderShipping { get; set; } = new Lazy<List<Order>>(() => new List<Order>());
        public Lazy<List<Order>> orderSuccess { get; set; } = new Lazy<List<Order>>(() => new List<Order>());
        public Lazy<List<OrderDetail>> orderDetail { get; set; } = new Lazy<List<OrderDetail>>(() => new List<OrderDetail>());
        public Lazy<Order> order { get; set; } = new Lazy<Order>(() => new Order());
    }
}
