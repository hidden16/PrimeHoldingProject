﻿namespace PrimeHoldingProject.Infrastructure.Abstractions.Contracts
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
