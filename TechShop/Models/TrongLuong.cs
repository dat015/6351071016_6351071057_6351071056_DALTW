namespace TechShop.Models
{
    public class TrongLuong
    {
        public int TrongLuongId { get; set; }
        public string SoKg { get; set; }

        // Các cấu hình liên quan
        public ICollection<CauHinh> CauHinhProducts { get; set; }
    }
}
