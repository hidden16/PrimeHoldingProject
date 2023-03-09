using PrimeHoldingProject.Infrastructure.Abstractions.Models;
using System.ComponentModel.DataAnnotations;
using static PrimeHoldingProject.Infrastructure.Constants.InfrastructureConstants.ManagerConstant;


namespace PrimeHoldingProject.Infrastructure.Data.Models
{
    public class Manager : BaseDeletableModel
    {
        [Key]
        public Guid Id { get; set; }
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
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
