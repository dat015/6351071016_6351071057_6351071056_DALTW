using TechShop.Models;

namespace TechShop.DTO
{
    public class ConfigDto
    {
        public int SpecId { get; set; }
        public int ProductId { get; set; }
        public int ConfigId { get; set; }
        public CPU CPU { get; set; }
        public RAM RAM { get; set; }
        public Pin Pin { get; set; }
        public ODia ODia { get; set; }
        public CardDoHoa CardDoHoa { get; set; }
        public TrongLuong TrongLuong { get; set; }
        public ManHinh ManHinh { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }
    }

}
