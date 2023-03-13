using System.ComponentModel.DataAnnotations;

namespace PrimeHoldingProject.Core.Models.Task
{
    public class CreateTaskViewModel
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime DueDate { get; set; }
    }
}
 