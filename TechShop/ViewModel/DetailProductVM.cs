using TechShop.DTO;
using TechShop.Models;

namespace TechShop.ViewModel
{
    public class DetailProductVM
    {
        public Specs product { get; set; }
        
        public List<Product> products { get; set; } = new List<Product>();
        public List<ConfigDto> cauHinhs { get; set; } = new List<ConfigDto>();
        public CauHinh config { get; set; }
    }
}
