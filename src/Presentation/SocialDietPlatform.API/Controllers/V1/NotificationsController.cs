using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Features.Notifications.Commands.SendNotification;
using SocialDietPlatform.Application.Features.Notifications.Queries.GetUserNotifications;
using System.Security.Claims;

namespace SocialDietPlatform.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Kullanıcının bildirimlerini getir
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<NotificationDto>>>> GetNotifications(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = GetCurrentUserId();
        var query = new GetUserNotificationsQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<NotificationDto>>.ErrorResult(result.Error));

        return Ok(ApiResponse<PagedResult<NotificationDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Okunmamış bildirim sayısını getir
    /// </summary>
    [HttpGet("unread-count")]
    public async Task<ActionResult<ApiResponse<int>>> GetUnreadNotificationCount()
    {
        var userId = GetCurrentUserId();
        // GetUnreadNotificationCountQuery implementation needed

        return Ok(ApiResponse<int>.SuccessResult(0));
    }

    /// <summary>
    /// Bildirimi okundu olarak işaretle
    /// </summary>
    [HttpPut("{id}/mark-read")]
    public async Task<ActionResult<ApiResponse<bool>>> MarkNotificationAsRead(Guid id)
    {
        // MarkNotificationAsReadCommand implementation needed
        return Ok(ApiResponse<bool>.SuccessResult(true, "Bildirim okundu olarak işaretlendi"));
    }

    /// <summary>
    /// Tüm bildirimleri okundu olarak işaretle
    /// </summary>
    [HttpPut("mark-all-read")]
    public async Task<ActionResult<ApiResponse<bool>>> MarkAllNotificationsAsRead()
    {
        var userId = GetCurrentUserId();
        // MarkAllNotificationsAsReadCommand implementation needed

        return Ok(ApiResponse<bool>.SuccessResult(true, "Tüm bildirimler okundu olarak işaretlendi"));
    }

    /// <summary>
    /// Bildirim gönder (Admin/Diyetisyen için)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Dietitian")]
    public async Task<ActionResult<ApiResponse<bool>>> SendNotification([FromBody] SendNotificationCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.ErrorResult(result.Error));

        return Ok(ApiResponse<bool>.SuccessResult(true, "Bildirim başarıyla gönderildi"));
    }

    /// <summary>
    /// Bildirimi sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteNotification(Guid id)
    {
        // DeleteNotificationCommand implementation needed
        return Ok(ApiResponse<bool>.SuccessResult(true, "Bildirim başarıyla silindi"));
    }

    /// <summary>
    /// Bildirim ayarlarını getir
    /// </summary>
    [HttpGet("settings")]
    public async Task<ActionResult<ApiResponse<object>>> GetNotificationSettings()
    {
        // GetNotificationSettingsQuery implementation needed
        var settings = new
        {
            EmailNotifications = true,
            PushNotifications = true,
            LikeNotifications = true,
            CommentNotifications = true,
            FollowNotifications = true,
            DietPlanNotifications = true
        };

        return Ok(ApiResponse<object>.SuccessResult(settings));
    }

    /// <summary>
    /// Bildirim ayarlarını güncelle
    /// </summary>
    [HttpPut("settings")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateNotificationSettings([FromBody] object settings)
    {
        // UpdateNotificationSettingsCommand implementation needed
        return Ok(ApiResponse<bool>.SuccessResult(true, "Bildirim ayarları başarıyla güncellendi"));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}