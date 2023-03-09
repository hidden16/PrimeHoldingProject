using Microsoft.AspNetCore.Identity;
using PrimeHoldingProject.Infrastructure.Abstractions.Contracts;
using System.ComponentModel.DataAnnotations;

namespace PrimeHoldingProject.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>, IDeletableEntity
    {
        public ApplicationUser()
        {
            IsDeleted = false;
        }
        [Required]
        [PersonalData]
        public string FirstName { get; set; }
        [Required]
        [PersonalData]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public DateTime BirthDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
