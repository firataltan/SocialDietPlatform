using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface IFollowRepository : IBaseRepository<Follow>
{
    Task<IEnumerable<Follow>> GetFollowersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Follow>> GetFollowingByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);
    Task<Follow?> GetFirstOrDefaultAsync(Expression<Func<Follow, bool>> predicate, CancellationToken cancellationToken = default);
} 