using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
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
    private readonly ILikeRepository _likeRepository;

    public LikePostCommandHandler(
        IPostRepository postRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService,
        ILikeRepository likeRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _likeRepository = likeRepository;
    }

    public async Task<Result<bool>> Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Önce mevcut like'ı kontrol et
            var existingLike = await _likeRepository.GetFirstOrDefaultAsync(
                l => l.PostId == request.PostId && l.UserId == request.UserId && !l.IsDeleted);

            if (existingLike != null)
            {
                // Unlike
                existingLike.IsDeleted = true;
                existingLike.DeletedAt = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success(false);
            }

            // Post ve User'ı kontrol et
            var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
            if (post == null)
            {
                return Result.Failure<bool>("Gönderi bulunamadı.");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure<bool>("Kullanıcı bulunamadı.");
            }

            // Like
            var like = new Like
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                PostId = request.PostId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _likeRepository.AddAsync(like, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send notification to post owner
            if (post.UserId != request.UserId)
            {
                try
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
                catch
                {
                    // Bildirim gönderme hatası olsa bile like işlemi başarılı sayılır
                }
            }

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Beğeni işlemi sırasında bir hata oluştu: {ex.Message}");
        }
    }
}