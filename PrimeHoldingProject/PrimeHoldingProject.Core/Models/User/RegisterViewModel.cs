using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static PrimeHoldingProject.Infrastructure.Constants.ErrorMessageConstants;
using static PrimeHoldingProject.Infrastructure.Constants.InfrastructureConstants.ApplicationUserConstant;


namespace PrimeHoldingProject.Core.Models.User
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = NameErrorMessage)]
        [PersonalData]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength, ErrorMessage = NameErrorMessage)]
        [PersonalData]
        public string LastName { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = PasswordErrorMessage)]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
