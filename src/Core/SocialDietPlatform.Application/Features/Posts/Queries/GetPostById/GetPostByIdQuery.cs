using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.Posts.Queries.GetPostById;

public record GetPostByIdQuery : IRequest<Result<PostDto>>
{
    public Guid PostId { get; init; }
    public Guid? CurrentUserId { get; init; }
}