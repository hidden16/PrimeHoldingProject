using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;
using PrimeHoldingProject.Infrastructure.Data.Models;

namespace PrimeHoldingProject.UserServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        public UserService(IRepository<ApplicationUser> userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<UserEmployeeViewModel> GetUserEmployeeInfoAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }

            return new UserEmployeeViewModel()
            {
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                Salary = 0
            };
        }
    }
}
