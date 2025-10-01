using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Features.Foods.Queries.GetFoods;

namespace SocialDietPlatform.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FoodsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FoodsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tüm yiyecekleri getir
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<FoodDto>>>> GetFoods()
    {
        var query = new GetFoodsQuery();
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return StatusCode(500, ApiResponse<IEnumerable<FoodDto>>.ErrorResult("Yiyecekler getirilirken bir hata oluştu.", errors: new List<string> { result.Error }));
        }

        return Ok(ApiResponse<IEnumerable<FoodDto>>.SuccessResult(result.Value, message: "Yiyecekler başarıyla getirildi."));
    }

    // Yiyecekleri getirecek GET endpoint'i buraya gelecek
} 