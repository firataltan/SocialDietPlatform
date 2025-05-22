using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;

namespace SocialDietPlatform.Application.Features.Users.Commands.FollowUser;

public record FollowUserCommand : IRequest<Result<bool>>
{
    public Guid FollowerId { get; init; }
    public Guid FollowingId { get; init; }
}