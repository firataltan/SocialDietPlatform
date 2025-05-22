using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Recipes.Commands.UpdateRecipe;

public record UpdateRecipeCommand : IRequest<Result<Recipe>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Instructions { get; init; } = string.Empty;
    public int PrepTimeMinutes { get; init; }
    public int CookTimeMinutes { get; init; }
    public int Servings { get; init; }
    public string? ImageUrl { get; init; }
    public Guid CategoryId { get; init; }
}

public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Result<Recipe>>
{
    private readonly IRecipeRepository _recipeRepository;

    public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Result<Recipe>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetByIdAsync(request.Id);
        if (recipe == null)
            return Result<Recipe>.Failure("Tarif bulunamadı.");

        recipe.Name = request.Name;
        recipe.Description = request.Description;
        recipe.Instructions = request.Instructions;
        recipe.PrepTimeMinutes = request.PrepTimeMinutes;
        recipe.CookTimeMinutes = request.CookTimeMinutes;
        recipe.Servings = request.Servings;
        recipe.ImageUrl = request.ImageUrl;
        recipe.CategoryId = request.CategoryId;

        await _recipeRepository.UpdateAsync(recipe);
        return Result<Recipe>.Success(recipe);
    }
} 