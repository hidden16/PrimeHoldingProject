using PrimeHoldingProject.Core.Models.Employee;
using PrimeHoldingProject.Core.Models.Task;
using PrimeHoldingProject.Core.Models.User;

namespace PrimeHoldingProject.Core.Contracts
{
    public interface IEmployeeService
    {
        Task BecomeEmployeeAsync(UserEmployeeViewModel model, Guid userId);
        Task<UserEmployeeViewModel> GetEmployeeInformation(Guid userId);
        Task EditEmployee(UserEmployeeViewModel model, Guid userId);
        Task QuitJobAsync(Guid userId);
        Task<IEnumerable<TaskViewModel>> MyTasksAsync(Guid userId);
        Task<IEnumerable<EmployeeViewModel>> AllEmployeesAsync();
        Task<IEnumerable<EmployeeRanklistViewModel>> EmployeeRanklistAsync();
    }
}
