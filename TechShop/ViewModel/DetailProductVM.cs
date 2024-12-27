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
        public int? Rating { get; set; } // Tổng số sao
        public int? ReviewCount { get; set; } // Số lượng đánh giá

        public List<Review> Reviews { get; set; }
        public string AdminReply { get; set; } // Thêm thuộc tính này
        public DateTime? AdminReplyDate { get; set; }
        public List<Product> RelatedProducts { get; set; } 

    }
}
