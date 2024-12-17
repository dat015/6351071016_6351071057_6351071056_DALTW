namespace TechShop.Models
{
    public class ODia
    {
        public int ODiaId { get; set; }
        public string TenODia { get; set; }
        public decimal AdditionalPrice { get; set; }

        // Các cấu hình liên quan
        public ICollection<CauHinh> CauHinhProducts { get; set; }
    }
}
