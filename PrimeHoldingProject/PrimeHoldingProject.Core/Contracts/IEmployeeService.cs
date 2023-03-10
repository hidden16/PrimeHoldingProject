using PrimeHoldingProject.Core.Models.User;

namespace PrimeHoldingProject.Core.Contracts
{
    public interface IEmployeeService
    {
        Task BecomeEmployeeAsync(UserEmployeeViewModel model, Guid userId);
    }
}
