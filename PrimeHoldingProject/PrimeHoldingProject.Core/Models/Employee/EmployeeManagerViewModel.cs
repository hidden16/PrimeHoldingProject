namespace PrimeHoldingProject.Core.Models.Employee
{
    public class EmployeeManagerViewModel
    {
        public Guid ManagerId { get; set; }
        public string ManagerFirstName { get; set; } = null!;
        public string ManagerLastName { get; set; } = null!;
    }
}
