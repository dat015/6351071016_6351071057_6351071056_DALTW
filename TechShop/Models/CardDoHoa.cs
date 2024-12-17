namespace TechShop.Models
{
    public class CardDoHoa
    {
        public int CardDoHoaId { get; set; }
        public string TenCardDoHoa { get; set; }
        public decimal AdditionalPrice { get; set; }
        // Các cấu hình liên quan
        public ICollection<CauHinh> CauHinhProducts { get; set; }
    }
}
