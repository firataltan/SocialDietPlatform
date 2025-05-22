using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;

namespace SocialDietPlatform.Domain.Entities;

public class DietPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TargetCalories { get; set; }
    public Guid UserId { get; set; }
    public Guid? DietitianId { get; set; }

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual User? Dietitian { get; set; }
    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
}
