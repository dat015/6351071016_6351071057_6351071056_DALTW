using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class provinces
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string full_name { get; set; }
        public string full_name_en { get; set; }
        public string code_name { get; set; }
       

        public int administrative_unit_id { get;set; }
        [ForeignKey("administrative_unit_id")]
        public administrative_units administrative_Units { get; set; }
        public int administrative_region_id { get; set; }
        [ForeignKey("administrative_region_id")]
        public administrative_regions administrative_Regions { get; set; }



    }
}
