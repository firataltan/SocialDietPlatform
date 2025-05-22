using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;
using System.Linq.Expressions;

namespace SocialDietPlatform.Persistence.Repositories;

public class FollowRepository : BaseRepository<Follow>, IFollowRepository
{
    private readonly ApplicationDbContext _context;

    public FollowRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Follow>> GetFollowersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .Include(f => f.Follower)
            .Where(f => f.FollowingId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Follow>> GetFollowingByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .Include(f => f.Following)
            .Where(f => f.FollowerId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId, cancellationToken);
    }
} 