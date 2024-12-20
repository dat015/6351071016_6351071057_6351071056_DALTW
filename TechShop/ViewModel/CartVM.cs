using TechShop.Models;
using TechShop.Data;

namespace TechShop.ViewModel
{
    public class CartVM
    {
        
        public IEnumerable<CartDetail> ListCart { get; set; }
      
        public IEnumerable<PaymentMethod> paymentMethods { get; set; }

        public ShoppingCart cart { get; set; }

        public decimal Total { get; set; }
      
    }
}
