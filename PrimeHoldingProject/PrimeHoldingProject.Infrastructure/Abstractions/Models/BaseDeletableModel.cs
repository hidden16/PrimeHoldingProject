using PrimeHoldingProject.Infrastructure.Abstractions.Contracts;

namespace PrimeHoldingProject.Infrastructure.Abstractions.Models
{
    public class BaseDeletableModel : BaseModel, IDeletableEntity
    {
        public BaseDeletableModel()
        {
            IsDeleted = false;
        }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
