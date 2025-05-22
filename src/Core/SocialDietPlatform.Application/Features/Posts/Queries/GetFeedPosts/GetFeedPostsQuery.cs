using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.Posts.Queries.GetFeedPosts;

public record GetFeedPostsQuery : IRequest<Result<PagedResult<PostDto>>>
{
    public Guid UserId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
