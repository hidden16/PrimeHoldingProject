namespace PrimeHoldingProject.Core.Models.Task
{
    public class TaskForEditViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
}
