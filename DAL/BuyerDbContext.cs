using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BuyerDbContext : IdentityDbContext<ApplicationUser>
    {
        public BuyerDbContext(DbContextOptions<BuyerDbContext> options) : base(options) { 
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            base.OnModelCreating(builder);
        }


        public DbSet<Tb_Affiliates> Tb_Affiliates { get; set; }

        public DbSet<Tb_AffiliateParameter> Tb_AffiliateParameters { get; set; }
    }
}
