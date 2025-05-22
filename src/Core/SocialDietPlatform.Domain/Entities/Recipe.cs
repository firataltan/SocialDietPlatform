using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;

namespace SocialDietPlatform.Domain.Entities;

public class Recipe : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }

    // Navigation Properties
    public virtual Category Category { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<RecipeIngredient> Ingredients { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Like> Likes { get; set; }

    public int TotalTimeMinutes => PrepTimeMinutes + CookTimeMinutes;
    public decimal TotalCalories => Ingredients.Sum(i => i.Calories);
    public decimal CaloriesPerServing => Servings > 0 ? TotalCalories / Servings : 0;
}