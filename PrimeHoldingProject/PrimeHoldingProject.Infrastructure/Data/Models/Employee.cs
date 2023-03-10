using PrimeHoldingProject.Infrastructure.Abstractions.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeHoldingProject.Infrastructure.Data.Models
{
    public class Employee : BaseDeletableModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Salary { get; set; }
        [Required]
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
