using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
 
namespace SocialDietPlatform.Application.Features.Recipes.Queries.GetRecipes
{
    public record GetRecipesQuery : IRequest<Result<IEnumerable<RecipeDto>>>;
} 