using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;

namespace PrimeHoldingProject.Infrastructure.Data.Configuration
{
    public class SeedRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = EmployeeConstant,
                    NormalizedName = EmployeeConstant.ToUpper()
                },
                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = ManagerConstant,
                    NormalizedName = ManagerConstant.ToUpper()
                });
        }
    }
}
