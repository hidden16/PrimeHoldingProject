using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeHoldingProject.Infrastructure.Data.Configuration;
using PrimeHoldingProject.Infrastructure.Data.Models;
using System.Security.Cryptography.X509Certificates;

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
            // Apply the configuration when creating the first migration or once when updating the database to the latest migration, after that comment the configuration to prevent collision in migrations.
            builder.ApplyConfiguration(new SeedRoleConfiguration());

            builder.Entity<ApplicationUser>()
                .HasOne(e => e.Employee)
                .WithOne(au => au.ApplicationUser)
                .HasForeignKey<Employee>(au=>au.ApplicationUserId);

            builder.Entity<ApplicationUser>()
               .HasOne(e => e.Manager)
               .WithOne(au => au.ApplicationUser)
               .HasForeignKey<Manager>(au => au.ApplicationUserId);

            builder.Entity<Employee>()
                .HasOne(x => x.Manager)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
                

            base.OnModelCreating(builder);
        }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

    }
}