using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrimeHoldingProject.Infrastructure.Abstractions.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "decimal(7,2)")]
        public decimal Salary { get; set; }
        public Guid? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; } 
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
