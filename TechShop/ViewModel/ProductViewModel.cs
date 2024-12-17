using TechShop.Models;

namespace TechShop.ViewModel
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public Category Cate { get; set; }
        public ProductViewModel(List<Product> Products, Category Cate)
        {
            this.Products = Products;
            this.Cate = Cate;
        }
    }
}
