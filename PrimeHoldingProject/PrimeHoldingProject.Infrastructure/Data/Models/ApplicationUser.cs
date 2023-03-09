using Microsoft.AspNetCore.Identity;
using PrimeHoldingProject.Infrastructure.Abstractions.Contracts;
using System.ComponentModel.DataAnnotations;
using static PrimeHoldingProject.Infrastructure.Constants.InfrastructureConstants.ApplicationUserConstant;

namespace PrimeHoldingProject.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>, IDeletableEntity
    {
        public ApplicationUser()
        {
            IsDeleted = false;
        }
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        [PersonalData]
        public string FirstName { get; set; }
        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [PersonalData]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public DateTime BirthDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public Guid? ManagerId { get; set; }
        public Manager? Manager { get; set; }
    }
}
