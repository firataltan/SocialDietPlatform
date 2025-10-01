using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities; // Add using for Recipe entity

namespace SocialDietPlatform.Application.Features.Recipes.Queries.GetRecipes
{
    public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, Result<IEnumerable<RecipeDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Result<IEnumerable<RecipeDto>>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetAllAsync(); // Assuming GetAllAsync exists and works

            var recipeDtos = recipes.Select(r => RecipeDto.FromEntity(r)).ToList(); // Use FromEntity if available

            return Result<IEnumerable<RecipeDto>>.Success(recipeDtos);
        }
    }
} 