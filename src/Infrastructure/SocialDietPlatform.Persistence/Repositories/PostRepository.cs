using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Domain.Enums;
using SocialDietPlatform.Persistence.Context;

namespace SocialDietPlatform.Persistence.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Post>> GetUserPostsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.User)
            .Include(p => p.Recipe)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Post>> GetFeedPostsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        // Kullanıcının takip ettiği kişilerin gönderileri + kendi gönderileri
        var followingUserIds = await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.FollowingId)
            .ToListAsync(cancellationToken);

        followingUserIds.Add(userId);

        return await _dbSet
            .Include(p => p.User)
            .Include(p => p.Recipe)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .Where(p => followingUserIds.Contains(p.UserId))
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Post>> GetPostsByTypeAsync(PostType type, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.User)
            .Include(p => p.Recipe)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .Where(p => p.Type == type)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Post?> GetPostWithDetailsAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.User)
            .Include(p => p.Recipe)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
    }
}
