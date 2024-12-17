using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Category Name is required")]
        public string CategoryName { get; set; }
        [MaxLength(255,ErrorMessage ="The value of Description can't exceed 255 characters.")]
        public string Description {  get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
