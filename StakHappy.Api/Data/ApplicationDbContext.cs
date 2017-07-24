using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StakHappy.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<Models.ApplicationUser> // OpenIddictDbContext
    {
        public ApplicationDbContext() : base() {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var connStr = System.Configuration.ConfigurationManager.ConnectionStrings["StakHappyIdentityConnection"].ConnectionString;
            builder.UseSqlServer(connStr);
            base.OnConfiguring(builder);
        }
    }
}
