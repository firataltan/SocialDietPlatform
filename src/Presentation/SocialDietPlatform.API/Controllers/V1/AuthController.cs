using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Features.Auth.Commands.Login;
using SocialDietPlatform.Application.Features.Auth.Commands.Register;

namespace SocialDietPlatform.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Kullanıcı kayıt işlemi
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(ApiResponse<object>.ErrorResult(result.Error));

        return Ok(ApiResponse<object>.SuccessResult(result.Value, "Kayıt işlemi başarılı"));
    }

    /// <summary>
    /// Kullanıcı giriş işlemi
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(ApiResponse<LoginResponse>.ErrorResult(result.Error));

        return Ok(ApiResponse<LoginResponse>.SuccessResult(result.Value, "Giriş işlemi başarılı"));
    }
}