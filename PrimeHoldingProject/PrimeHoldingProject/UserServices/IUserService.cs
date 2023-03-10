using PrimeHoldingProject.Core.Models.User;

namespace PrimeHoldingProject.UserServices
{
    public interface IUserService
    {
        Task<UserEmployeeViewModel> GetUserEmployeeInfoAsync(Guid userId);
    }
}
