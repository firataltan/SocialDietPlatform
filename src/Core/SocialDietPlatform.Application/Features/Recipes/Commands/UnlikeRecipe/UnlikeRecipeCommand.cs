using MediatR;
using Microsoft.AspNetCore.Http;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Repositories;
using System.Security.Claims;

namespace SocialDietPlatform.Application.Features.Recipes.Commands.UnlikeRecipe;

public record UnlikeRecipeCommand : IRequest<Result>
{
    public Guid RecipeId { get; init; }
}

public class UnlikeRecipeCommandHandler : IRequestHandler<UnlikeRecipeCommand, Result>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnlikeRecipeCommandHandler(
        IRecipeRepository recipeRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _recipeRepository = recipeRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(UnlikeRecipeCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Result.Failure("Kullanıcı girişi yapılmamış.");

        var userId = Guid.Parse(userIdClaim);
        var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

        if (recipe == null)
            return Result.Failure("Tarif bulunamadı.");

        var like = recipe.Likes.FirstOrDefault(l => l.UserId == userId);
        if (like == null)
            return Result.Failure("Bu tarifi beğenmemişsiniz.");

        recipe.Likes.Remove(like);
        await _recipeRepository.UpdateAsync(recipe);

        return Result.Success();
    }
} 