using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            base.OnModelCreating(builder);
        }

        public DbSet<Tb_Sell> Tb_Sells { get; set; }

        public DbSet<Tb_Affiliates> Tb_Affiliates { get; set; }

        public DbSet<Tb_AffiliateParameter> Tb_AffiliateParameters { get; set; }
    }
}
