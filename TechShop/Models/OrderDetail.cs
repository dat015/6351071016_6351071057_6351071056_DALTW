using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    [PrimaryKey(nameof(specId), nameof(OrderId))]
    public class OrderDetail
    {
        public int specId { get; set; }
        [ForeignKey("specId")]
        public Specs Specs { get; set; }
        public int OrderId { get; set; }
        [Required]
        [ForeignKey("OrderId")]
        public Order order { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}
