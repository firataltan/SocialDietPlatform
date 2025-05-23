using MediatR;
using Microsoft.AspNetCore.Http;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using System.Security.Claims;

namespace SocialDietPlatform.Application.Features.Recipes.Commands.LikeRecipe;

public record LikeRecipeCommand : IRequest<Result>
{
    public Guid RecipeId { get; init; }
}

public class LikeRecipeCommandHandler : IRequestHandler<LikeRecipeCommand, Result>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LikeRecipeCommandHandler(
        IRecipeRepository recipeRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _recipeRepository = recipeRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(LikeRecipeCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Result.Failure("Kullanıcı girişi yapılmamış.");

        var userId = Guid.Parse(userIdClaim);
        var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

        if (recipe == null)
            return Result.Failure("Tarif bulunamadı.");

        if (recipe.Likes.Any(l => l.UserId == userId))
            return Result.Failure("Bu tarifi zaten beğenmişsiniz.");

        recipe.Likes.Add(new Like { UserId = userId });
        await _recipeRepository.UpdateAsync(recipe);

        return Result.Success();
    }
} 