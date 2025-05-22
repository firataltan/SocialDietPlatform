using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface IRecipeRepository : IBaseRepository<Recipe>
{
    Task<Recipe> GetByIdAsync(Guid id);
    Task<IEnumerable<Recipe>> GetAllAsync();
    Task<IEnumerable<Recipe>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Recipe>> GetUserRecipesAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recipe>> GetRecipesByCategoryAsync(Guid categoryId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Recipe?> GetRecipeWithIngredientsAsync(Guid recipeId, CancellationToken cancellationToken = default);
    Task<Recipe> AddAsync(Recipe recipe);
    Task<Recipe> UpdateAsync(Recipe recipe);
    Task DeleteAsync(Recipe recipe);
}