using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;
using System.Linq.Expressions;

namespace SocialDietPlatform.Persistence.Repositories;

public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
{
    private readonly ApplicationDbContext _context;

    public RecipeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Recipe> AddAsync(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe> UpdateAsync(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task DeleteAsync(Recipe recipe)
    {
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task<Recipe> GetByIdAsync(Guid id)
    {
        return await _context.Recipes
            .Include(r => r.User)
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return await _context.Recipes
            .Include(r => r.User)
            .Include(r => r.Ingredients)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recipe>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recipe>> GetUserRecipesAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Where(r => r.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Recipe>> GetRecipesByCategoryAsync(Guid categoryId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Where(r => r.CategoryId == categoryId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Where(r => r.Name.Contains(searchTerm) || r.Description.Contains(searchTerm))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Recipe?> GetRecipeWithIngredientsAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId, cancellationToken);
    }
} 