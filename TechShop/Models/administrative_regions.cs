using System.ComponentModel.DataAnnotations;

namespace TechShop.Models
{
    public class administrative_regions
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string code_en { get; set; }
        public string code_name_en { get; set; }
    }
}
