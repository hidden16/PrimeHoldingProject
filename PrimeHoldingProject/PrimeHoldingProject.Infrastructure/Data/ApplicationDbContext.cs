using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeHoldingProject.Infrastructure.Data.Models;

namespace PrimeHoldingProject.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasOne(e => e.Employee)
                .WithOne(au => au.ApplicationUser)
                .HasForeignKey<Employee>(au=>au.ApplicationUserId);

            builder.Entity<ApplicationUser>()
               .HasOne(e => e.Manager)
               .WithOne(au => au.ApplicationUser)
               .HasForeignKey<Manager>(au => au.ApplicationUserId);

            base.OnModelCreating(builder);
        }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

    }
}