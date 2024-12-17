using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string distrcit {  get; set; }
        public string provice { get; set; }
        public string ward { get; set; }
        public string address {  get; set; }

    }
}
