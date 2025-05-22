using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;
using System.Linq.Expressions;

namespace SocialDietPlatform.Persistence.Repositories;

public class MealRepository : BaseRepository<Meal>, IMealRepository
{
    private readonly ApplicationDbContext _context;

    public MealRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Meal> AddAsync(Meal meal)
    {
        await _context.Meals.AddAsync(meal);
        await _context.SaveChangesAsync();
        return meal;
    }

    public async Task<Meal> UpdateAsync(Meal meal)
    {
        _context.Meals.Update(meal);
        await _context.SaveChangesAsync();
        return meal;
    }

    public async Task DeleteAsync(Meal meal)
    {
        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
    }

    public async Task<Meal> GetByIdAsync(Guid id)
    {
        return await _context.Meals
            .Include(m => m.MealFoods)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Meal>> GetAllAsync()
    {
        return await _context.Meals
            .Include(m => m.MealFoods)
            .ToListAsync();
    }

    public async Task<IEnumerable<Meal>> GetByDietPlanIdAsync(Guid dietPlanId, CancellationToken cancellationToken = default)
    {
        return await _context.Meals
            .Where(m => m.DietPlanId == dietPlanId)
            .Include(m => m.MealFoods)
            .ToListAsync(cancellationToken);
    }
} 