using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;

namespace SocialDietPlatform.Application.Features.Posts.Commands.DeletePost;

public record DeletePostCommand : IRequest<Result<bool>>
{
    public Guid PostId { get; init; }
    public Guid UserId { get; init; }
}