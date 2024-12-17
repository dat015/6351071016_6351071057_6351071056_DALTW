using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class districts
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string full_name { get; set; }
        public string full_name_en { get; set; }
        public string code_name { get; set; }
        public string province_code { get; set; }
        [ForeignKey("province_code")]
        public provinces province { get; set; }
        public int administrative_unit_id { get; set; }
        [ForeignKey("administrative_unit_id")]

        public administrative_units administrative_Units { get; set; }

    }
}
