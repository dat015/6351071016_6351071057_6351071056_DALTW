namespace TechShop.Models
{
    public class Pin
    {
        public int PinId { get; set; }
        public string DungLuongPin { get; set; }
        public decimal AdditionalPrice { get; set; }

        // Các cấu hình liên quan
        public ICollection<CauHinh> CauHinhProducts { get; set; }
    }
}
