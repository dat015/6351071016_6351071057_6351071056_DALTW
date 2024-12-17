namespace TechShop.Models
{
    public class CPU
    {
        public int CPUId { get; set; }
        public string TenCPU { get; set; }
        public decimal AdditionalPrice { get; set; }


        // Các cấu hình liên quan
        public ICollection<CauHinh> CauHinhProducts { get; set; }
    }
}
