using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Recipes.Queries.GetRecipesByCategory;

public class GetRecipesByCategoryQuery : IRequest<Result<IEnumerable<Recipe>>>
{
    public Guid CategoryId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetRecipesByCategoryQueryHandler : IRequestHandler<GetRecipesByCategoryQuery, Result<IEnumerable<Recipe>>>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetRecipesByCategoryQueryHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Result<IEnumerable<Recipe>>> Handle(GetRecipesByCategoryQuery request, CancellationToken cancellationToken)
    {
        var recipes = await _recipeRepository.GetRecipesByCategoryAsync(request.CategoryId, request.PageNumber, request.PageSize, cancellationToken);
        return Result<IEnumerable<Recipe>>.Success(recipes);
    }
} 