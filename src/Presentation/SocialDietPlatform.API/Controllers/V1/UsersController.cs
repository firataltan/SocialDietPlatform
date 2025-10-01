using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Features.Users.Commands.FollowUser;
using SocialDietPlatform.Application.Features.Users.Commands.UpdateProfile;
using SocialDietPlatform.Application.Features.Users.Queries.GetUserProfile;
using SocialDietPlatform.Application.Features.Users.Queries.SearchUsers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SocialDietPlatform.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Yeni record tanımı
    public record UploadResult(string Url);

    /// <summary>
    /// Kullanıcı profili getir
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserProfile(Guid userId)
    {
        var currentUserId = GetCurrentUserId();
        var query = new GetUserProfileQuery { UserId = userId, CurrentUserId = currentUserId };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return NotFound(ApiResponse<UserDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<UserDto>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Profili güncelle
    /// </summary>
    [HttpPut("profile")]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var userId = GetCurrentUserId();
        var commandWithUserId = command with { UserId = userId };

        var result = await _mediator.Send(commandWithUserId);

        if (result.IsFailure)
            return BadRequest(ApiResponse<UserDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<UserDto>.SuccessResult(result.Value, "Profil başarıyla güncellendi"));
    }

    /// <summary>
    /// Profil resmi yükle
    /// </summary>
    [HttpPost("upload-profile-image")]
    public async Task<ActionResult<ApiResponse<UploadResult>>> UploadProfileImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<UploadResult>.ErrorResult("Dosya seçilmedi."));
        }

        try
        {
            // Dosya adını güvenli hale getir (isteğe bağlı ama önerilir)
            var trustedFileNameForDisplay = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(trustedFileNameForDisplay);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            // Yükleme klasörünü belirle (wwwroot altında bir klasör olabilir)
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profilepictures");

            // Klasör yoksa oluştur
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Dosyayı sunucuya kaydet
            var filePath = Path.Combine(uploadPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Kaydedilen dosyanın URL'ini oluştur
            var fileUrl = $"/profilepictures/{uniqueFileName}";

            // Başarılı yanıtı UploadResult kullanarak döndür
            return Ok(ApiResponse<UploadResult>.SuccessResult(new UploadResult(fileUrl), "Profil resmi başarıyla yüklendi."));
        }
        catch (Exception ex)
        {
            // Hata yönetimi
            return StatusCode(500, ApiResponse<UploadResult>.ErrorResult($"Dosya yüklenirken bir hata oluştu: {ex.Message}"));
        }
    }

    /// <summary>
    /// Kullanıcı ara
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<PagedResult<UserDto>>>> SearchUsers(
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchUsersQuery
        {
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<UserDto>>.ErrorResult(result.Error));

        return Ok(ApiResponse<PagedResult<UserDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Kullanıcıyı takip et/takibi bırak
    /// </summary>
    [HttpPost("{userId}/follow")]
    public async Task<ActionResult<ApiResponse<bool>>> FollowUser(Guid userId)
    {
        var followerId = GetCurrentUserId();
        var command = new FollowUserCommand { FollowerId = followerId, FollowingId = userId };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.ErrorResult(result.Error));

        var message = result.Value ? "Kullanıcı takip edildi" : "Kullanıcı takibi bırakıldı";
        return Ok(ApiResponse<bool>.SuccessResult(result.Value, message));
    }

    /// <summary>
    /// Mevcut kullanıcının profilini getir
    /// </summary>
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetCurrentUserProfile()
    {
        var userId = GetCurrentUserId();
        var query = new GetUserProfileQuery { UserId = userId };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return NotFound(ApiResponse<UserDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<UserDto>.SuccessResult(result.Value));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}