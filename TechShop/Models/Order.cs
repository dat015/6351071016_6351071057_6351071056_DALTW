using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechShop.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
        [Required]
        [StringLength(100 , ErrorMessage = "Status can't exceed 100 characters")]
        public string StatusPayment { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Method status can't exceed 100 characters")]
        public string? StatusShipping { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Status can't exceed 100 characters")]
        public string Note { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } 

        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime PaymentDate { get; set; }
       
        public int PaymentMethodId {  get; set; }
        
        [ForeignKey("PaymentMethodId")]
        public PaymentMethod PaymentMethod { get; set; }
        
        [Required]
        public string distrcit { get; set; }

        [Required]
        public string provice { get; set; }

        [Required]

        public string ward { get; set; }

        [Required]

        public string address { get; set; }

        [Required]

        public string phoneNumber { get; set; }
        [Required]

        public string Name { get; set; }


    }
}
