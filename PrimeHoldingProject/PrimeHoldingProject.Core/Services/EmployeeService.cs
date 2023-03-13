using Ganss.XSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Employee;
using PrimeHoldingProject.Core.Models.Task;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;
using PrimeHoldingProject.Infrastructure.Data.Models;
using System.Net.Mail;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;

namespace PrimeHoldingProject.Core.Services
{
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

        public async Task<IEnumerable<EmployeeViewModel>> AllEmployeesAsync()
        {
            var employees = await employeeRepository.All()
                .Include(x => x.Tasks)
                .ToListAsync();

            var employeesDto = new List<EmployeeViewModel>();
            foreach (var employee in employees)
            {
                employeesDto.Add(new EmployeeViewModel
                {
                    Id = employee.Id,
                    BirthDate = employee.BirthDate,
                    DoneTasksCount = employee.Tasks.Count,
                    EmailAddress = employee.EmailAddress,
                    FullName = employee.FullName,
                    PhoneNumber = employee.PhoneNumber,
                    Salary = employee.Salary
                });
            }
            return employeesDto;
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

            var manager = await managerRepository.GetByIdAsync(model.ManagerId);
            if (manager == null)
            {
                throw new ArgumentException();
            }

            manager.Employees.Add(employee);

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
            employee.ModifiedOn = DateTime.UtcNow;
            if (employee.ManagerId != model.ManagerId)
            {
                var currentManager = await managerRepository.GetByIdAsync(employee.ManagerId);
                currentManager.Employees.Remove(employee);
                var newManager = await managerRepository.GetByIdAsync(model.ManagerId);
                newManager.Employees.Add(employee);
                employee.ManagerId = model.ManagerId;
            }
            user.PhoneNumber = Sanitize(model.PhoneNumber);
            user.BirthDate = model.BirthDate;
            user.Email = Sanitize(model.EmailAddress);
            employeeRepository.Update(employee);
            await employeeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeRanklistViewModel>> EmployeeRanklistAsync()
        {
            var employees = await employeeRepository.All()
                .Include(x => x.Tasks)
                .ToListAsync();

            var year = DateTime.Today.Year;
            var month = DateTime.Today.Month;

            var firstDayOfPreviousMonth = new DateTime(year, month, 1).AddMonths(-1);
            var lastDayOfPreviousMonth = new DateTime(year, month, 1).AddDays(-1);

            var employeesDto = new List<EmployeeRanklistViewModel>();
            foreach (var employee in employees)
            {
                employeesDto.Add(new EmployeeRanklistViewModel
                {
                    FullName = employee.FullName,
                    TasksDoneCount = employee.Tasks
                    .Where(x => x.CompletionDate > firstDayOfPreviousMonth && x.CompletionDate < lastDayOfPreviousMonth)
                    .Count()
                });
            }
            return employeesDto.OrderByDescending(x => x.TasksDoneCount).Take(5);
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

        public async Task<IEnumerable<TaskViewModel>> MyTasksAsync(Guid userId)
        {
            var employee = await employeeRepository
                .AllExpression(x => x.ApplicationUserId == userId && x.IsDeleted == false)
                .Include(x => x.Tasks)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentException();
            }

            var tasks = new List<TaskViewModel>();
            foreach (var task in employee.Tasks)
            {
                tasks.Add(new TaskViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate.ToShortDateString(),
                    CompletionDate = task.CompletionDate.ToString()
                });
            }
            return tasks;
        }

        public async System.Threading.Tasks.Task QuitJobAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var employee = await employeeRepository.GetByIdAsync(user.EmployeeId);
            employee.ApplicationUserId = null;
            user.EmployeeId = null;
            await employeeRepository.SetDeletedByIdAsync(employee.Id);
            await userManager.RemoveFromRoleAsync(user, EmployeeConstant);
            await employeeRepository.SaveChangesAsync();
        }

        private string Sanitize(string text)
        {
            return sanitizer.Sanitize(text);
        }
    }
}
