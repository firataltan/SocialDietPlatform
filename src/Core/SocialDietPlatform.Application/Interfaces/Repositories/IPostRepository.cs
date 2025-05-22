using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<IEnumerable<Post>> GetUserPostsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetFeedPostsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetPostsByTypeAsync(Domain.Enums.PostType type, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Post?> GetPostWithDetailsAsync(Guid postId, CancellationToken cancellationToken = default);
}