using Ganss.XSS;
using Microsoft.AspNetCore.Identity;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Employee;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;
using PrimeHoldingProject.Infrastructure.Data.Models;
using System.Net.Mail;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;

namespace PrimeHoldingProject.Core.Services
{
    // TODO: After implementing the manager and after edit the manager changes remove from the first and add to the second.
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Manager> managerRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private HtmlSanitizer sanitizer = new HtmlSanitizer();
        public EmployeeService(IRepository<Employee> employeeRepository,
            IRepository<ApplicationUser> userRepository,
            IRepository<Manager> managerRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.employeeRepository = employeeRepository;
            this.userRepository = userRepository;
            this.managerRepository = managerRepository;
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
                EmailAddress = Sanitize(model.EmailAddress),
                FullName = Sanitize(model.FullName),
                PhoneNumber = Sanitize(model.PhoneNumber),
                Salary = model.Salary,
                ManagerId = model.ManagerId
            };
            user.EmployeeId = employee.Id;
            user.PhoneNumber = Sanitize(model.PhoneNumber);
            user.BirthDate = model.BirthDate;
            user.Email = Sanitize(model.EmailAddress);
            await employeeRepository.AddAsync(employee);
            await userManager.AddToRoleAsync(user, EmployeeConstant);
            await employeeRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task EditEmployee(UserEmployeeViewModel model, Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var employee = await employeeRepository.GetByIdAsync(user.EmployeeId);
            employee.BirthDate = model.BirthDate;
            employee.EmailAddress = Sanitize(model.EmailAddress);
            employee.FullName = Sanitize(model.FullName);
            employee.PhoneNumber = Sanitize(model.PhoneNumber);
            employee.Salary = model.Salary;
            employee.ManagerId = model.ManagerId;
            user.PhoneNumber = Sanitize(model.PhoneNumber);
            user.BirthDate = model.BirthDate;
            user.Email = Sanitize(model.EmailAddress);
            employeeRepository.Update(employee);
            await employeeRepository.SaveChangesAsync();
        }

        public async Task<UserEmployeeViewModel> GetEmployeeInformation(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }

            var employee = await employeeRepository.GetByIdAsync(user.EmployeeId);
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

            return new UserEmployeeViewModel
            {
                FullName = employee.FullName,
                BirthDate = employee.BirthDate,
                EmailAddress = employee.EmailAddress,
                PhoneNumber = employee.PhoneNumber,
                Salary = employee.Salary,
                Managers = managersDto
            };
        }

        private string Sanitize(string text)
        {
            return sanitizer.Sanitize(text);
        }
    }
}
