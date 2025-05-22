using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<Comment>> GetPostCommentsAsync(Guid postId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetUserCommentsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
}