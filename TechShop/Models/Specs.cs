using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class Specs
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ConfigId { get; set; }
        public decimal Price { get; set; } = 0;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("ConfigId")]
        public virtual CauHinh Config { get; set; }
    }
}
