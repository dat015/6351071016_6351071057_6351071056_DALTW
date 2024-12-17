using System.ComponentModel.DataAnnotations;

namespace TechShop.Models
{
    public class administrative_units
    {
        [Key]
        public int id { get; set; }
        public string full_name { get; set; }
        public string full_name_en { get; set; }
        public string short_name { get; set; }
        public string short_name_en { get; set; }
        public string code_name { get; set; }
        public string code_name_en { get; set; }


    }
}
