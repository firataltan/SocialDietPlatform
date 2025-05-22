using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.Features.Posts.Commands.LikePost;

public class LikePostCommandHandler : IRequestHandler<LikePostCommand, Result<bool>>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public LikePostCommandHandler(
        IPostRepository postRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<Result<bool>> Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostWithDetailsAsync(request.PostId, cancellationToken);
        if (post == null)
        {
            return Result.Failure<bool>("Gönderi bulunamadı.");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<bool>("Kullanıcı bulunamadı.");
        }

        var existingLike = post.Likes.FirstOrDefault(l => l.UserId == request.UserId);

        if (existingLike != null)
        {
            // Unlike
            post.Likes.Remove(existingLike);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(false);
        }
        else
        {
            // Like
            var like = new Like
            {
                UserId = request.UserId,
                PostId = request.PostId
            };

            post.Likes.Add(like);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send notification to post owner
            if (post.UserId != request.UserId)
            {
                await _notificationService.SendNotificationAsync(
                    post.UserId,
                    "Gönderin Beğenildi",
                    $"{user.FullName} gönderinizi beğendi",
                    NotificationType.Like,
                    request.PostId,
                    "Post",
                    cancellationToken);
            }

            return Result.Success(true);
        }
    }
}