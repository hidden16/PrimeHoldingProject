﻿using PrimeHoldingProject.Infrastructure.Abstractions.Models;
using System.ComponentModel.DataAnnotations;

namespace PrimeHoldingProject.Infrastructure.Data.Models
{
    public class Task : BaseDeletableModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;

        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
