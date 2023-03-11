using PrimeHoldingProject.Core.Models.User;

namespace PrimeHoldingProject.Core.Contracts
{
    public interface IEmployeeService
    {
        Task BecomeEmployeeAsync(UserEmployeeViewModel model, Guid userId);
        Task<UserEmployeeViewModel> GetEmployeeInformation(Guid userId);
        Task EditEmployee(UserEmployeeViewModel model, Guid userId);
    }
}
