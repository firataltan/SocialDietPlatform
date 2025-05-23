using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task SendNotificationAsync(Guid userId, string title, string message, NotificationType type,
        Guid? relatedEntityId = null, string? relatedEntityType = null, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            RelatedEntityId = relatedEntityId,
            RelatedEntityType = relatedEntityType,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification, cancellationToken);
    }

    public async Task SendBulkNotificationAsync(IEnumerable<Guid> userIds, string title, string message, NotificationType type,
        CancellationToken cancellationToken = default)
    {
        var notifications = userIds.Select(userId => new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        foreach (var notification in notifications)
        {
            await _notificationRepository.AddAsync(notification, cancellationToken);
        }
    }

    public async Task SendRealTimeNotificationAsync(Guid userId, object notification, CancellationToken cancellationToken = default)
    {
        // TODO: Implement real-time notification using SignalR or similar technology
        await Task.CompletedTask;
    }
} 