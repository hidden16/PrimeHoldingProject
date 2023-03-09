using PrimeHoldingProject.Infrastructure.Abstractions.Contracts;

namespace PrimeHoldingProject.Infrastructure.Abstractions.Models
{
    public class BaseModel : IAuditInfo
    {
        public BaseModel()
        {
            CreatedOn = DateTime.UtcNow;
        }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
