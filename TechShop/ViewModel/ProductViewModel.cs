using TechShop.Models;

namespace TechShop.ViewModel
{
    public class ProductViewModel
    {
        public List<Specs> specs { get; set; }
        public List<Product> Products { get; set; }
        public Category Cate { get; set; }
        public List<CPU> CPUs { get; set; }
        public List<ManHinh> ManHinhs { get; set; }
        public List<ODia> ODia { get; set; }
        public List<CardDoHoa> cardDoHoas { get; set; }
        public List<RAM> RAMs { get; set; }
        public List<Brand> Brands { get; set; }
        
        public ProductViewModel(List<Product> Products, Category Cate)
        {
            this.Products = Products;
            this.Cate = Cate;
        }
        public ProductViewModel()
        {
            
        }
    }
}
