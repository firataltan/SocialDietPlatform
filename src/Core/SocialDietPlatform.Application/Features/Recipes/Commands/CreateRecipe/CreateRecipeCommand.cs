using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.Recipes.Commands.CreateRecipe;

public record CreateRecipeCommand : IRequest<Result<RecipeDto>>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Instructions { get; init; } = string.Empty;
    public int PreparationTime { get; init; }
    public int CookingTime { get; init; }
    public int Servings { get; init; }
    public string? ImageUrl { get; init; }
    public Guid UserId { get; init; }
    public Guid CategoryId { get; init; }
    public List<CreateRecipeIngredientDto> Ingredients { get; init; } = new();
}

public record CreateRecipeIngredientDto
{
    public Guid FoodId { get; init; }
    public decimal Quantity { get; init; }
    public string Unit { get; init; } = string.Empty;
}
