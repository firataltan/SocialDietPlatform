using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;

namespace SocialDietPlatform.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.FirstName.Contains(searchTerm) ||
                       u.LastName.Contains(searchTerm) ||
                       u.UserName!.Contains(searchTerm))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .Where(f => f.FollowingId == userId)
            .Select(f => f.Follower)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.Following)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId, cancellationToken);
    }
}