using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechShop.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public byte? Rating { get; set; }

        [MaxLength]
        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now; 
    }
}
