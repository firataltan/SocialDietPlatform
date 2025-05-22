using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.Users.Queries.GetUserProfile;

public record GetUserProfileQuery : IRequest<Result<UserDto>>
{
    public Guid UserId { get; init; }
    public Guid? CurrentUserId { get; init; }
}
