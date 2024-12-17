using Microsoft.EntityFrameworkCore;
using TechShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TechShop.Data
{
    public class ApplicationDb1Context : DbContext
    {

        public ApplicationDb1Context(DbContextOptions<ApplicationDb1Context> options) : base(options)
        { 
         
        }
        public DbSet<wards> wards { get; set; }
        public DbSet<provinces> provinces { get; set; }
        public DbSet<districts> districts { get; set; }
        public DbSet<administrative_units> administrative_Units { get; set; }
        public DbSet<administrative_regions> administrative_Regions { get; set; }

    }
}
