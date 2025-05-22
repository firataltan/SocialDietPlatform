using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Recipes.Queries.GetRecipe;

public class GetRecipeQuery : IRequest<Result<Recipe>>
{
    public Guid RecipeId { get; set; }
}

public class GetRecipeQueryHandler : IRequestHandler<GetRecipeQuery, Result<Recipe>>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetRecipeQueryHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Result<Recipe>> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe == null)
            return Result<Recipe>.Failure("Recipe not found");

        return Result<Recipe>.Success(recipe);
    }
} 