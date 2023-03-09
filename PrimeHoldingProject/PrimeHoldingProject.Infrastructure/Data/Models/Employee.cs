using PrimeHoldingProject.Infrastructure.Abstractions.Models;
using System.ComponentModel.DataAnnotations;

namespace PrimeHoldingProject.Infrastructure.Data.Models
{
    public class Employee : BaseDeletableModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string EmailAddress { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
}
