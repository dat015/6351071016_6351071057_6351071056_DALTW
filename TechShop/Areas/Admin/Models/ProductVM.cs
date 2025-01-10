using TechShop.Models;

namespace TechShop.Areas.Admin.Models
{
    public class ProductVM
    {
        public Lazy<Task<List<Specs>>> Specs { get; set; }
        public Lazy<Task<List<Product>>> Products { get; set; }
        public Lazy<Task<List<Category>>> Cate { get; set; }
        public Lazy<Task<List<CPU>>> CPUs { get; set; }
        public Lazy<Task<List<ManHinh>>> ManHinhs { get; set; }
        public Lazy<Task<List<ODia>>> ODia { get; set; }
        public Lazy<Task<List<CardDoHoa>>> CardDoHoas { get; set; }
        public Lazy<Task<List<Pin>>> Pins { get; set; }
        public Lazy<Task<List<TrongLuong>>> trongLuongs { get; set; }
        public Lazy<Task<List<Review>>> reviews { get; set; }
        public Lazy<Task<List<RAM>>> RAMs { get; set; }
        public Lazy<Task<List<Brand>>> Brands { get; set; }

        public int CPUId { get; set; }
        public int RAMId { get; set; }
        public int CardDoHoaId { get; set; }
        public int ODiaId { get; set; }
        public int ManHinhId { get; set; }
        public int PinId { get; set; }
        public int TrongLuongId { get; set; }
        public int ProductId { get; set; }
        public ProductVM()
        {
            Specs = new Lazy<Task<List<Specs>>>();
            Products = new Lazy<Task<List<Product>>>();
            Cate = new Lazy<Task<List<Category>>>();
            CPUs = new Lazy<Task<List<CPU>>>();
            ManHinhs = new Lazy<Task<List<ManHinh>>>();
            ODia = new Lazy<Task<List<ODia>>>();
            CardDoHoas = new Lazy<Task<List<CardDoHoa>>>();
            Pins = new Lazy<Task<List<Pin>>>();
            RAMs = new Lazy<Task<List<RAM>>>();
            Brands = new Lazy<Task<List<Brand>>>();
        }
    }
}
