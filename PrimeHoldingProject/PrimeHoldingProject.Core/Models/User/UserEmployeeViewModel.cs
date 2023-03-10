using System.ComponentModel.DataAnnotations;

namespace PrimeHoldingProject.Core.Models.User
{
    public class UserEmployeeViewModel
    {
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
        public decimal Salary { get; set; }
    }
}
