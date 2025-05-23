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
    public int PreparationTime { get; set; }
    public int CookingTime { get; set; }
    public int Servings { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public decimal TotalCalories { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TotalTimeMinutes { get; set; }
    public decimal CaloriesPerServing { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public ICollection<RecipeIngredientDto> Ingredients { get; set; } = new List<RecipeIngredientDto>();
    public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();

    public static RecipeDto FromEntity(Recipe recipe)
    {
        return new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            PreparationTime = recipe.PreparationTime,
            CookingTime = recipe.CookingTime,
            Servings = recipe.Servings,
            ImageUrl = recipe.ImageUrl,
            TotalCalories = recipe.TotalCalories,
            CategoryId = recipe.CategoryId,
            UserId = recipe.UserId,
            CreatedAt = recipe.CreatedAt,
            UpdatedAt = recipe.UpdatedAt
        };
    }
}
