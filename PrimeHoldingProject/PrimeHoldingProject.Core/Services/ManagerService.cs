using Ganss.XSS;
using Microsoft.AspNetCore.Identity;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Employee;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;
using PrimeHoldingProject.Infrastructure.Data.Models;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;

namespace PrimeHoldingProject.Core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Manager> managerRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private HtmlSanitizer sanitizer = new HtmlSanitizer();
        public ManagerService(IRepository<ApplicationUser> userRepository,
            IRepository<Manager> managerRepository,
            IRepository<Employee> employeeRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.managerRepository = managerRepository;
            this.employeeRepository= employeeRepository;
            this.userManager = userManager;
        }
        public async System.Threading.Tasks.Task BecomeManagerAsync(UserManagerViewModel model, Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var manager = new Manager
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = user.Id,
                BirthDate = model.BirthDate,
                EmailAddress = Sanitize(model.EmailAddress),
                FirstName = Sanitize(model.FirstName),
                LastName = Sanitize(model.LastName),
                Salary = model.Salary
            };
            user.ManagerId = manager.Id;
            user.FirstName = Sanitize(model.FirstName);
            user.LastName = Sanitize(model.LastName);
            user.Email = Sanitize(model.EmailAddress);

            await managerRepository.AddAsync(manager);
            await userManager.AddToRoleAsync(user, ManagerConstant);
            await managerRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task EditManager(UserManagerViewModel model, Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var manager = await managerRepository.GetByIdAsync(user.ManagerId);
            manager.BirthDate = model.BirthDate;
            manager.EmailAddress = Sanitize(model.EmailAddress);
            manager.FirstName = Sanitize(model.FirstName);
            manager.LastName = Sanitize(model.LastName);
            manager.Salary = model.Salary;
            manager.ModifiedOn = DateTime.UtcNow;
            user.BirthDate = model.BirthDate;
            user.Email = Sanitize(model.EmailAddress);
            managerRepository.Update(manager);
            await managerRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task FireEmployeeAsync(Guid employeeId)
        {
            var employee = await employeeRepository.GetByIdAsync(employeeId);
            var user = await userRepository.GetByIdAsync(employee.ApplicationUserId);
            if (employee == null || user == null)
            {
                throw new ArgumentException();
            }
            employee.ApplicationUserId = null;
            user.EmployeeId = null;
            var employeeManager = await managerRepository.GetByIdAsync(employee.ManagerId);
            employeeManager.Employees.Remove(employee);
            await userManager.RemoveFromRoleAsync(user, EmployeeConstant);
            await employeeRepository.SetDeletedByIdAsync(employee.Id);
            await employeeRepository.SaveChangesAsync();

        }

        public async Task<UserManagerViewModel> GetManagerInformation(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }

            var manager = await managerRepository.GetByIdAsync(user.ManagerId);

            return new UserManagerViewModel
            {
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                BirthDate = manager.BirthDate,
                EmailAddress = manager.EmailAddress,
                Salary = manager.Salary
            };
        }

        public async System.Threading.Tasks.Task QuitJobAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var manager = await managerRepository.GetByIdAsync(user.ManagerId);
            manager.ApplicationUserId = null;
            user.ManagerId = null;
            await userManager.RemoveFromRoleAsync(user, ManagerConstant);
            await managerRepository.SetDeletedByIdAsync(manager.Id);
            await managerRepository.SaveChangesAsync();
        }

        private string Sanitize(string text)
        {
            return sanitizer.Sanitize(text);
        }
    }
}
