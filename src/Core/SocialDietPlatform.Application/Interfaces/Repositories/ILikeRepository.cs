using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<Like> GetFirstOrDefaultAsync(Expression<Func<Like, bool>> predicate, CancellationToken cancellationToken = default);
        Task<Like> AddAsync(Like entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<Like>> GetAllAsync(Expression<Func<Like, bool>> predicate = null, CancellationToken cancellationToken = default);
        Task<Like> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Like entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Like entity, CancellationToken cancellationToken = default);
    }
} 