using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace TechShop.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public string Image { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Name of banch can't exceed 100 characters")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name of banch can't exceed 50 characters")]
        [Display(Name = "Email")]

        public string Email { get; set; }
       
  
        [Required]
        [StringLength(10, ErrorMessage = "Name of banch can't exceed 10 characters")]   
        [Display(Name ="Số điện thoại")]

        public string? PhoneNumber { get; set; }
        [Required]
        public int RoleId { get; set; } 

        public bool IsLock { get; set; } = false;

        public bool IsEmailConfirmed { get; set; } = false;

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name of banch can't exceed 50 characters")]
        public string Password { get; set; }
        public string RandomKey { get; set; }
    }
}
