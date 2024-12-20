using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechShop.Repository.Validation;
using Newtonsoft.Json;


namespace TechShop.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required, MaxLength(1000, ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required,Range(0,1000000000, ErrorMessage ="Yêu cầu nhập giá sản phẩm")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceBase { get; set; }
        [Required, Range(0,1000, ErrorMessage ="Yêu cầu nhập số lượng trong kho")]
        public int StockQuantity { get; set; }
        [Required, MaxLength(10000, ErrorMessage ="Yêu cầu nhập mô tả")]
        public string Description { get; set; }
        public string Img { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int warranty { get; set; }
        
        public Category CategoryOfProducts { get; set; }
        public Brand BrandOfProducts { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpLoad { get; set; }
        public ICollection<CauHinh> CauHinhProducts { get; set; }
        public ICollection<Specs> Specifications { get; set; }

    }
}
