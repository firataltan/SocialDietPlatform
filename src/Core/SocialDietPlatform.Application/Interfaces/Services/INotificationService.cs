using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendNotificationAsync(Guid userId, string title, string message, NotificationType type,
        Guid? relatedEntityId = null, string? relatedEntityType = null, CancellationToken cancellationToken = default);
    Task SendBulkNotificationAsync(IEnumerable<Guid> userIds, string title, string message, NotificationType type,
        CancellationToken cancellationToken = default);
    Task SendRealTimeNotificationAsync(Guid userId, object notification, CancellationToken cancellationToken = default);
}
