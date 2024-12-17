using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace TechShop.ViewModel
{
    public class LoginVM
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = ("*"))]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = ("*"))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
