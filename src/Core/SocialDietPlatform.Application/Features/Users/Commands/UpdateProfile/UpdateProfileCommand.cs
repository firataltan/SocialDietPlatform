using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.Features.Users.Commands.UpdateProfile;

public record UpdateProfileCommand : IRequest<Result<UserDto>>
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime? DateOfBirth { get; init; }
    public string? Bio { get; init; }
    public decimal? Weight { get; init; }
    public decimal? Height { get; init; }
    public decimal? TargetWeight { get; init; }
    public string? ProfilePictureUrl { get; init; }
}