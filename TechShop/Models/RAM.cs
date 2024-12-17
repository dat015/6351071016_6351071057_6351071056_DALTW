namespace TechShop.Models
{
    public class RAM
    {
        public int RAMId { get; set; }
        public string DungLuong { get; set; }
        public decimal AdditionalPrice { get; set; }

        // Các cấu hình liên quan
        public ICollection<CauHinh> CauHinhProducts { get; set; }
    }
}
