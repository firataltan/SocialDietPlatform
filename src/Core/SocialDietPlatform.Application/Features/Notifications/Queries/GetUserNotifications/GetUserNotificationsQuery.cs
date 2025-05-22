using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using System.Linq;

namespace SocialDietPlatform.Application.Features.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQuery : IRequest<Result<PagedResult<NotificationDto>>>
{
    public Guid UserId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, Result<PagedResult<NotificationDto>>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<PagedResult<NotificationDto>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetUserNotificationsAsync(request.UserId, request.PageNumber, request.PageSize, cancellationToken);
        var notificationDtos = notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            UserId = n.UserId,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            AdditionalData = n.AdditionalData,
            IsRead = n.IsRead,
            ReadAt = n.ReadAt,
            RelatedEntityId = n.RelatedEntityId,
            RelatedEntityType = n.RelatedEntityType,
            CreatedAt = n.CreatedAt
        }).ToList();

        var pagedResult = new PagedResult<NotificationDto>
        {
            Items = notificationDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = notificationDtos.Count()
        };
        return Result<PagedResult<NotificationDto>>.Success(pagedResult);
    }
} 