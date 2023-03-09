namespace PrimeHoldingProject.Infrastructure.Abstractions.Contracts
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
