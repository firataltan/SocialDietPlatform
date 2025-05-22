using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);
}