namespace TechShop.Areas.Customer.ViewModel
{
    public class ChangePasswordVM
    {
        public string CurrentPassword { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
