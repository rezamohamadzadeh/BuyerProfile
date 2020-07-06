using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            base.OnModelCreating(builder);
        }

        public DbSet<Tb_Support> Tb_Supports { get; set; }
        public DbSet<Tb_SupportType> Tb_SupportTypes { get; set; }

    }
}
