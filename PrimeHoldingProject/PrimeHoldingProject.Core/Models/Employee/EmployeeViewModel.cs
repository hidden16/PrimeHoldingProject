using System.ComponentModel.DataAnnotations;

namespace PrimeHoldingProject.Core.Models.Employee
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public decimal Salary { get; set; }
        public int DoneTasksCount { get; set; }
    }
}
