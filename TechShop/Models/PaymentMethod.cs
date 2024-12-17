using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechShop.Models
{
    public class PaymentMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Method name can't exceed 100 characters")]
        public string? MethodName { get; set; }

       
    }
}
