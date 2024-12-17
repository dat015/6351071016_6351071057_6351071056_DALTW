namespace TechShop.Models
{
    public class ManHinh
    {
        public int ManHinhId { get; set; }
        public string KichThuocManHinh { get; set; }
        public decimal AdditionalPrice { get; set; }

        // Các cấu hình liên quan
        public ICollection<CauHinh> CauHinhProducts { get; set; }
    }
}
