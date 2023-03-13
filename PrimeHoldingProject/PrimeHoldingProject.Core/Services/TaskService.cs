using Ganss.XSS;
using Microsoft.EntityFrameworkCore;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Task;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;
using PrimeHoldingProject.Infrastructure.Data.Models;

namespace PrimeHoldingProject.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Infrastructure.Data.Models.Task> taskRepository;
        private readonly IRepository<Manager> managerRepository;
        private readonly IRepository<Employee> employeeRepository;
        private HtmlSanitizer sanitizer = new HtmlSanitizer();
        public TaskService(IRepository<ApplicationUser> userRepository,
            IRepository<Infrastructure.Data.Models.Task> taskRepository,
            IRepository<Manager> managerRepository,
            IRepository<Employee> employeeRepository)
        {
            this.userRepository = userRepository;
            this.taskRepository = taskRepository;
            this.managerRepository = managerRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<TaskViewModel>> AllTasksAsync()
        {
            var tasks = await taskRepository.All()
                .Where(x => x.CompletionDate == null && x.DueDate > DateTime.UtcNow)
                .ToListAsync();

            var tasksDto = new List<TaskViewModel>();

            foreach (var task in tasks)
            {
                tasksDto.Add(new TaskViewModel
                {
                    Id = task.Id,
                    Description = task.Description,
                    DueDate = task.DueDate.ToShortDateString(),
                    Title = task.Title
                });
            }
            return tasksDto;
        }

        public async Task<IEnumerable<TaskViewModel>> AllTasksForManagersAsync()
        {
            var tasks = await taskRepository.All()
                .Where(x => x.CompletionDate == null)
                .ToListAsync();

            var tasksDto = new List<TaskViewModel>();

            foreach (var task in tasks)
            {
                tasksDto.Add(new TaskViewModel
                {
                    Id = task.Id,
                    Description = task.Description,
                    DueDate = task.DueDate.ToShortDateString(),
                    Title = task.Title
                });
            }
            return tasksDto;
        }

        public async System.Threading.Tasks.Task CreateTaskAsync(CreateTaskViewModel model, Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var manager = await managerRepository.GetByIdAsync(user.ManagerId);
            if (manager == null)
            {
                throw new ArgumentException();
            }

            var task = new Infrastructure.Data.Models.Task
            {
                Id = Guid.NewGuid(),
                Title = Sanitize(model.Title),
                Description = Sanitize(model.Description),
                DueDate = model.DueDate,
                ManagerId = manager.Id
            };
            manager.Tasks.Add(task);

            await taskRepository.AddAsync(task);
            await taskRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid taskId)
        {
            await taskRepository.SetDeletedByIdAsync(taskId);
            await taskRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task EditTaskAsync(TaskForEditViewModel model)
        {
            var task = await taskRepository.GetByIdAsync(model.Id);
            if (task == null)
            {
                throw new ArgumentException();
            }

            task.Title = Sanitize(model.Title);
            task.Description = Sanitize(model.Description);
            task.DueDate = model.DueDate;
            task.ModifiedOn = DateTime.UtcNow;

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task FinishTaskAsync(Guid taskId, Guid userId)
        {
            var task = await taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new ArgumentException();
            }
            var employee = await employeeRepository
                .AllExpression(x => x.ApplicationUserId == userId && x.IsDeleted == false)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentException();
            }
            task.CompletionDate = DateTime.UtcNow;
            task.EmployeeId = employee.Id;
            employee.Tasks.Add(task);
            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();
        }

        public async Task<TaskForEditViewModel> GetTaskForEditAsync(Guid taskId)
        {
            var task = await taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new ArgumentException();
            }

            return new TaskForEditViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate
            };
        }

        private string Sanitize(string text)
        {
            return sanitizer.Sanitize(text);
        }
    }
}
