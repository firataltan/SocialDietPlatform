using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;
using System.Linq.Expressions;

namespace SocialDietPlatform.Persistence.Repositories;

public class DietPlanRepository : BaseRepository<DietPlan>, IDietPlanRepository
{
    private readonly ApplicationDbContext _context;

    public DietPlanRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DietPlan>> GetUserDietPlansAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.DietPlans.Where(d => d.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<DietPlan?> GetActiveDietPlanAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.DietPlans.FirstOrDefaultAsync(d => d.UserId == userId && !d.IsDeleted, cancellationToken);
    }

    public async Task<DietPlan?> GetDietPlanWithMealsAsync(Guid dietPlanId, CancellationToken cancellationToken = default)
    {
        return await _context.DietPlans.Include(d => d.Meals).FirstOrDefaultAsync(d => d.Id == dietPlanId, cancellationToken);
    }
} 