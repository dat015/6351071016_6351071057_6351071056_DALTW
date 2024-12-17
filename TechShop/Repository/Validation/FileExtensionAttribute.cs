using System.ComponentModel.DataAnnotations;

namespace TechShop.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object  value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower(); // 123.pnj
                string[] extensions = { ".jpg", ".png", ".jpeg" };

                bool result = extensions.Any(x => extension.Equals(x));

                if (!result)
                {
                    return new ValidationResult("Allowed extensions are .jpg or .pnj or .jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
