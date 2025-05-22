using Microsoft.EntityFrameworkCore;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Persistence.Context;
using System.Linq.Expressions;

namespace SocialDietPlatform.Persistence.Repositories;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnreadNotificationCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
    }

    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await _context.Notifications.FindAsync(new object[] { notificationId }, cancellationToken);
        if (notification != null)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync(cancellationToken);
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
} 