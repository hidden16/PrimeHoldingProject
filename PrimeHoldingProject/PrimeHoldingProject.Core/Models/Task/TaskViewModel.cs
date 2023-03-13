using System.ComponentModel.DataAnnotations;

namespace PrimeHoldingProject.Core.Models.Task
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DueDate { get; set; }
        public string? CompletionDate { get; set; }
    }
}
