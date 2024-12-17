using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class DetailAddress
    {
        public int userId { get; set; }
        
        [ForeignKey("Id")]
        public User user { get; set; }
        public int addressId { get; set; }
        
        [ForeignKey("Id")]
        public Address Address { get; set; }
    }
}
