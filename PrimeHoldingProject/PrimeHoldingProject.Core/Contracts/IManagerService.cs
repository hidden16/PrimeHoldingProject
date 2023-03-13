using PrimeHoldingProject.Core.Models.Task;
using PrimeHoldingProject.Core.Models.User;

namespace PrimeHoldingProject.Core.Contracts
{
    public interface IManagerService
    {
        Task BecomeManagerAsync(UserManagerViewModel model, Guid userId);
        Task QuitJobAsync(Guid userId);
        Task<UserManagerViewModel> GetManagerInformation(Guid userId);
        Task EditManager(UserManagerViewModel model, Guid userId);
        Task FireEmployeeAsync(Guid employeeId);
    }
}
