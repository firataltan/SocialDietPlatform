using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.Features.Notifications.Commands.SendNotification;

public class SendNotificationCommand : IRequest<Result<Notification>>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public string? AdditionalData { get; set; }

    public SendNotificationCommand()
    {
        Title = string.Empty;
        Message = string.Empty;
    }

    public SendNotificationCommand(string title, string message, Guid userId, NotificationType type, string? additionalData = null)
    {
        Title = title;
        Message = message;
        UserId = userId;
        Type = type;
        AdditionalData = additionalData;
    }
}

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Result<Notification>>
{
    private readonly INotificationRepository _notificationRepository;

    public SendNotificationCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<Notification>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            Type = request.Type,
            AdditionalData = request.AdditionalData,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        return Result<Notification>.Success(notification);
    }
} 