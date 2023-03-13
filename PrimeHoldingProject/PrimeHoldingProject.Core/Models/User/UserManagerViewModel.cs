using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PrimeHoldingProject.Infrastructure.Constants.InfrastructureConstants.ManagerConstant;

namespace PrimeHoldingProject.Core.Models.User
{
    public class UserManagerViewModel
    {
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Salary { get; set; }
    }
}
