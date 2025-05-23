using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, Result<RecipeDto>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IFoodRepository _foodRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateRecipeCommandHandler(
        IRecipeRepository recipeRepository,
        IFoodRepository foodRepository,
        ICategoryRepository categoryRepository)
    {
        _recipeRepository = recipeRepository;
        _foodRepository = foodRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<RecipeDto>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        // Validate category exists
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
            return Result<RecipeDto>.Failure("Kategori bulunamadı.");

        // Create recipe
        var recipe = new Recipe
        {
            Name = request.Name,
            Description = request.Description,
            Instructions = request.Instructions,
            PreparationTime = request.PreparationTime,
            CookingTime = request.CookingTime,
            Servings = request.Servings,
            ImageUrl = request.ImageUrl ?? string.Empty,
            CategoryId = request.CategoryId,
            UserId = request.UserId,
            Ingredients = new List<RecipeIngredient>()
        };

        // Add ingredients
        foreach (var ingredient in request.Ingredients)
        {
            var food = await _foodRepository.GetByIdAsync(ingredient.FoodId);
            if (food == null)
                return Result<RecipeDto>.Failure($"Yiyecek bulunamadı: {ingredient.FoodId}");

            var recipeIngredient = new RecipeIngredient
            {
                FoodId = ingredient.FoodId,
                Quantity = ingredient.Quantity,
                Unit = ingredient.Unit,
                Calories = food.NutritionalInfo.Calories * (ingredient.Quantity / 100) // Calculate calories based on quantity
            };

            recipe.Ingredients.Add(recipeIngredient);
        }

        // Calculate total calories
        recipe.TotalCalories = recipe.Ingredients.Sum(i => i.Calories);

        // Save recipe
        var createdRecipe = await _recipeRepository.AddAsync(recipe);

        // Map to DTO
        var recipeDto = RecipeDto.FromEntity(createdRecipe);
        return Result<RecipeDto>.Success(recipeDto);
    }
} 