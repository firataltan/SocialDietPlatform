using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;
using SocialDietPlatform.Domain.Enums;
using SocialDietPlatform.Domain.ValueObjects;

namespace SocialDietPlatform.Domain.Entities;

public class Meal : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public MealType Type { get; set; }
    public DateTime PlannedTime { get; set; }
    public DateTime? ConsumedTime { get; set; }
    public Guid DietPlanId { get; set; }

    // Navigation Properties
    public virtual DietPlan DietPlan { get; set; } = null!;
    public virtual ICollection<MealFood> MealFoods { get; set; } = new List<MealFood>();

    public bool IsConsumed => ConsumedTime.HasValue;
    public decimal TotalCalories => MealFoods.Sum(mf => mf.Calories);
}