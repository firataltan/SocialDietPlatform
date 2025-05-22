using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;

namespace SocialDietPlatform.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<UserDto>("Kullanıcı bulunamadı.");
        }

        var userDto = _mapper.Map<UserDto>(user);

        // Get follower/following counts
        var followers = await _userRepository.GetFollowersAsync(request.UserId, cancellationToken);
        var following = await _userRepository.GetFollowingAsync(request.UserId, cancellationToken);

        userDto.FollowersCount = followers.Count();
        userDto.FollowingCount = following.Count();

        // Check if current user is following this user
        if (request.CurrentUserId.HasValue && request.CurrentUserId != request.UserId)
        {
            userDto.IsFollowing = await _userRepository.IsFollowingAsync(
                request.CurrentUserId.Value,
                request.UserId,
                cancellationToken);
        }

        return Result.Success(userDto);
    }
}