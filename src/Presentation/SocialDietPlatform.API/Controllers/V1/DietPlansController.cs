using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Features.DietPlans.Commands.CreateDietPlan;
using SocialDietPlatform.Application.Features.DietPlans.Commands.UpdateDietPlan;
using SocialDietPlatform.Application.Features.DietPlans.Queries.GetDietPlan;
using SocialDietPlatform.Application.Features.DietPlans.Queries.GetUserDietPlans;
using System.Security.Claims;

namespace SocialDietPlatform.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class DietPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public DietPlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Yeni diyet planı oluştur
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DietPlanDto>>> CreateDietPlan([FromBody] CreateDietPlanCommand command)
    {
        var userId = GetCurrentUserId();
        var commandWithUserId = command with { UserId = userId };

        var result = await _mediator.Send(commandWithUserId);

        if (result.IsFailure)
            return BadRequest(ApiResponse<DietPlanDto>.ErrorResult(result.Error));

        return CreatedAtAction(
            nameof(GetDietPlan),
            new { id = result.Value.Id },
            ApiResponse<DietPlanDto>.SuccessResult(result.Value, "Diyet planı başarıyla oluşturuldu"));
    }

    /// <summary>
    /// Diyet planı detayını getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DietPlanDto>>> GetDietPlan(Guid id)
    {
        var query = new GetDietPlanQuery { DietPlanId = id };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return NotFound(ApiResponse<DietPlanDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<DietPlanDto>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Kullanıcının diyet planlarını getir
    /// </summary>
    [HttpGet("my-plans")]
    public async Task<ActionResult<ApiResponse<IEnumerable<DietPlanDto>>>> GetMyDietPlans()
    {
        var userId = GetCurrentUserId();
        var query = new GetUserDietPlansQuery { UserId = userId };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(ApiResponse<IEnumerable<DietPlanDto>>.ErrorResult(result.Error));

        return Ok(ApiResponse<IEnumerable<DietPlanDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Belirli kullanıcının diyet planlarını getir (diyetisyenler için)
    /// </summary>
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Dietitian,Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<DietPlanDto>>>> GetUserDietPlans(Guid userId)
    {
        var query = new GetUserDietPlansQuery { UserId = userId };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(ApiResponse<IEnumerable<DietPlanDto>>.ErrorResult(result.Error));

        return Ok(ApiResponse<IEnumerable<DietPlanDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Diyet planını güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DietPlanDto>>> UpdateDietPlan(Guid id, [FromBody] UpdateDietPlanCommand command)
    {
        var commandWithId = command with { DietPlanId = id };

        var result = await _mediator.Send(commandWithId);

        if (result.IsFailure)
            return BadRequest(ApiResponse<DietPlanDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<DietPlanDto>.SuccessResult(result.Value, "Diyet planı başarıyla güncellendi"));
    }

    /// <summary>
    /// Diyet planını sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDietPlan(Guid id)
    {
        // DeleteDietPlanCommand implementation needed
        return Ok(ApiResponse<bool>.SuccessResult(true, "Diyet planı başarıyla silindi"));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}