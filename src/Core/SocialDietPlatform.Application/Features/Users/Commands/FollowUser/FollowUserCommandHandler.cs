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
    private readonly IFollowRepository _followRepository;

    public FollowUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService,
        IFollowRepository followRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _followRepository = followRepository;
    }

    public async Task<Result<bool>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
            var isAlreadyFollowing = await _followRepository.IsFollowingAsync(request.FollowerId, request.FollowingId, cancellationToken);

            if (isAlreadyFollowing)
            {
                // Unfollow
                var existingFollow = await _followRepository.GetFirstOrDefaultAsync(
                    f => f.FollowerId == request.FollowerId && f.FollowingId == request.FollowingId && !f.IsDeleted,
                    cancellationToken);

                if (existingFollow != null)
                {
                    existingFollow.IsDeleted = true;
                    existingFollow.DeletedAt = DateTime.UtcNow;
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                return Result.Success(false);
            }
            else
            {
                // Follow
                var follow = new Follow
                {
                    Id = Guid.NewGuid(),
                    FollowerId = request.FollowerId,
                    FollowingId = request.FollowingId,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _followRepository.AddAsync(follow, cancellationToken);
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
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Takip işlemi sırasında bir hata oluştu: {ex.Message}");
        }
    }
}