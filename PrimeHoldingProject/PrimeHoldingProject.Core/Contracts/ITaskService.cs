using PrimeHoldingProject.Core.Models.Task;

namespace PrimeHoldingProject.Core.Contracts
{
    public interface ITaskService
    {
        Task CreateTaskAsync(CreateTaskViewModel model, Guid userId);
        Task<IEnumerable<TaskViewModel>> AllTasksAsync();
        Task<IEnumerable<TaskViewModel>> AllTasksForManagersAsync();
        Task FinishTaskAsync(Guid taskId, Guid userId);
        Task EditTaskAsync(TaskForEditViewModel model);
        Task<TaskForEditViewModel> GetTaskForEditAsync(Guid taskId);
        Task DeleteAsync(Guid taskId);
    }
}
