using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet_MVC_Training.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNet_MVC_Training.Areas.Identity.Data
{
    public class IdentityDataContext : IdentityDbContext<IdentityUser>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<UserTraining>()
              .HasKey(ut => new { ut.UserId, ut.TrainingID });
        }

        public DbSet<Training> Training { get; set; }
        public DbSet<UserTraining> UserTraining { get; set; }
    }
}
