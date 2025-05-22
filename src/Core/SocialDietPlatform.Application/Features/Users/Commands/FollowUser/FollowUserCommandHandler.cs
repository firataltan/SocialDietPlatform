using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.Features.Users.Commands.FollowUser;

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public FollowUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<Result<bool>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        // Business rule validation
        // var cannotFollowSelfRule = new UserCannotFollowThemselfRule(request.FollowerId, request.FollowingId);
        // cannotFollowSelfRule.CheckRule();

        var follower = await _userRepository.GetByIdAsync(request.FollowerId, cancellationToken);
        if (follower == null)
        {
            return Result.Failure<bool>("Takip eden kullanıcı bulunamadı.");
        }

        var following = await _userRepository.GetByIdAsync(request.FollowingId, cancellationToken);
        if (following == null)
        {
            return Result.Failure<bool>("Takip edilecek kullanıcı bulunamadı.");
        }

        // Check if already following
        var isAlreadyFollowing = await _userRepository.IsFollowingAsync(request.FollowerId, request.FollowingId, cancellationToken);

        if (isAlreadyFollowing)
        {
            // Unfollow logic would go here
            return Result.Success(false);
        }
        else
        {
            // Follow
            var follow = new Follow
            {
                FollowerId = request.FollowerId,
                FollowingId = request.FollowingId
            };

            // Add follow relationship (this would need to be added to a repository)
            // For now, we'll assume this is handled in the UserRepository

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send notification
            await _notificationService.SendNotificationAsync(
                request.FollowingId,
                "Yeni Takipçi",
                $"{follower.FullName} sizi takip etmeye başladı",
                NotificationType.Follow,
                request.FollowerId,
                "User",
                cancellationToken);

            return Result.Success(true);
        }
    }
}