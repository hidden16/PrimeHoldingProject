using Microsoft.AspNetCore.Identity;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;
using PrimeHoldingProject.Infrastructure.Data.Models;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;

namespace PrimeHoldingProject.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public EmployeeService(IRepository<Employee> employeeRepository,
            IRepository<ApplicationUser> userRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.employeeRepository = employeeRepository;
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        public async System.Threading.Tasks.Task BecomeEmployeeAsync(UserEmployeeViewModel model, Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = userId,
                BirthDate = model.BirthDate,
                EmailAddress = model.EmailAddress,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Salary = model.Salary
            };
            user.EmployeeId = employee.Id;
            await employeeRepository.AddAsync(employee);
            await userManager.AddToRoleAsync(user, EmployeeConstant);
            await employeeRepository.SaveChangesAsync();
        }
    }
}
