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
    public int PreparationTime { get; set; }
    public int CookingTime { get; set; }
    public int Servings { get; set; }
    public string ImageUrl { get; set; }
    public decimal TotalCalories { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }

    // Navigation Properties
    public virtual Category? Category { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<RecipeIngredient> Ingredients { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Like> Likes { get; set; }

    public Recipe()
    {
        Name = string.Empty;
        Description = string.Empty;
        Instructions = string.Empty;
        ImageUrl = string.Empty;
        Ingredients = new List<RecipeIngredient>();
        Comments = new List<Comment>();
        Likes = new List<Like>();
    }

    public Recipe(string name, string description, string instructions, string imageUrl, Guid categoryId, Guid userId)
    {
        Name = name;
        Description = description;
        Instructions = instructions;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
        UserId = userId;
        Ingredients = new List<RecipeIngredient>();
        Comments = new List<Comment>();
        Likes = new List<Like>();
    }

    public int TotalTimeMinutes => PreparationTime + CookingTime;
    public decimal CaloriesPerServing => Servings > 0 ? TotalCalories / Servings : 0;
    public int PrepTimeMinutes => PreparationTime;
    public int CookTimeMinutes => CookingTime;
}