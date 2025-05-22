using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;

namespace SocialDietPlatform.Domain.Entities;

public class MealFood : BaseEntity
{
    public Guid MealId { get; set; }
    public Guid FoodId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Calories { get; set; }

    // Navigation Properties
    public virtual Meal Meal { get; set; } = null!;
    public virtual Food Food { get; set; } = null!;
}