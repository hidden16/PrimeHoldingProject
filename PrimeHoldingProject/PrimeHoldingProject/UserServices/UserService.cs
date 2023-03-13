using PrimeHoldingProject.Core.Models.Employee;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;
using PrimeHoldingProject.Infrastructure.Data.Models;

namespace PrimeHoldingProject.UserServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Manager> managerRepository;

        public UserService(IRepository<ApplicationUser> userRepository,
            IRepository<Manager> managerRepository)
        {
            this.userRepository = userRepository;
            this.managerRepository = managerRepository;
        }
        public async Task<UserEmployeeViewModel> GetUserEmployeeInfoAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            var managers = managerRepository.All();
            List<EmployeeManagerViewModel> managersDto = new List<EmployeeManagerViewModel>();
            foreach (var manager in managers)
            {
                managersDto.Add(new EmployeeManagerViewModel
                {
                    ManagerId = manager.Id,
                    ManagerFirstName = manager.FirstName,
                    ManagerLastName = manager.LastName,
                });
            }
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
                Salary = 0,
                Managers = managersDto
            };
        }

        public async Task<UserManagerViewModel> GetUserManagerInfoAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }

            return new UserManagerViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                EmailAddress = user.Email,
                Salary = 0
            };
        }
    }
}
