using TechShop.Models;
using TechShop.Data;

namespace TechShop.ViewModel
{
    public class CartVM
    {
        
        public IEnumerable<CartDetail> ListCart { get; set; }
        public IEnumerable<districts> listDistrict { get; set; } 
        public IEnumerable<wards> listWards { get; set; }
        public IEnumerable<provinces> listProvices { get; set; }
        public IEnumerable<PaymentMethod> paymentMethods { get; set; }

        public ShoppingCart cart { get; set; }

        public decimal Total { get; set; }
      
    }
}
