using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;
using System.Linq.Expressions;

namespace SocialDietPlatform.Persistence.Repositories;

public class FoodRepository : BaseRepository<Food>, IFoodRepository
{
    private readonly ApplicationDbContext _context;

    public FoodRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Food> AddAsync(Food food)
    {
        await _context.Foods.AddAsync(food);
        await _context.SaveChangesAsync();
        return food;
    }

    public async Task<Food> UpdateAsync(Food food)
    {
        _context.Foods.Update(food);
        await _context.SaveChangesAsync();
        return food;
    }

    public async Task DeleteAsync(Food food)
    {
        _context.Foods.Remove(food);
        await _context.SaveChangesAsync();
    }

    public async Task<Food> GetByIdAsync(Guid id)
    {
        return await _context.Foods
            .Include(f => f.MealFoods)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Food>> GetAllAsync()
    {
        return await _context.Foods
            .Include(f => f.MealFoods)
            .ToListAsync();
    }

    public async Task<IEnumerable<Food>> GetByMealIdAsync(Guid mealId, CancellationToken cancellationToken = default)
    {
        return await _context.Foods
            .Where(f => f.MealFoods.Any(mf => mf.MealId == mealId))
            .Include(f => f.MealFoods)
            .ToListAsync(cancellationToken);
    }
} 