using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;
using SocialDietPlatform.Domain.ValueObjects;

namespace SocialDietPlatform.Domain.Entities;

public class Food : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public NutritionalInfo NutritionalInfo { get; set; } = null!;
    public string Unit { get; set; } = "gram"; // gram, ml, piece, etc.

    // Navigation Properties
    public virtual ICollection<MealFood> MealFoods { get; set; } = new List<MealFood>();
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}