using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.DTOs;

public class RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string? ImageUrl { get; set; }
    public int TotalTimeMinutes { get; set; }
    public decimal TotalCalories { get; set; }
    public decimal CaloriesPerServing { get; set; }
    public UserDto User { get; set; } = null!;
    public CategoryDto Category { get; set; } = null!;
    public List<RecipeIngredientDto> Ingredients { get; set; } = new();
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static RecipeDto FromEntity(Recipe recipe)
    {
        return new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            PrepTimeMinutes = recipe.PrepTimeMinutes,
            CookTimeMinutes = recipe.CookTimeMinutes,
            Servings = recipe.Servings,
            ImageUrl = recipe.ImageUrl,
            TotalTimeMinutes = recipe.TotalTimeMinutes,
            TotalCalories = recipe.TotalCalories,
            CaloriesPerServing = recipe.CaloriesPerServing,
            CategoryId = recipe.CategoryId,
            UserId = recipe.UserId,
            CreatedAt = recipe.CreatedAt,
            UpdatedAt = recipe.UpdatedAt
        };
    }
}
