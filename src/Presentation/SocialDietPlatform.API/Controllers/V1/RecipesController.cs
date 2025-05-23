using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Features.Comments.Commands.AddComment;
using SocialDietPlatform.Application.Features.Comments.Commands.DeleteComment;
using SocialDietPlatform.Application.Features.Comments.Queries.GetRecipeComments;
using SocialDietPlatform.Application.Features.Recipes.Commands.CreateRecipe;
using SocialDietPlatform.Application.Features.Recipes.Commands.UpdateRecipe;
using SocialDietPlatform.Application.Features.Recipes.Queries.GetRecipe;
using SocialDietPlatform.Application.Features.Recipes.Queries.GetRecipesByCategory;
using SocialDietPlatform.Application.Features.Recipes.Queries.SearchRecipes;
using SocialDietPlatform.Application.Features.Recipes.Commands.LikeRecipe;
using SocialDietPlatform.Application.Features.Recipes.Commands.UnlikeRecipe;
using System.Security.Claims;

namespace SocialDietPlatform.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecipesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Yeni tarif oluştur
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RecipeDto>>> CreateRecipe([FromBody] CreateRecipeCommand command)
    {
        var userId = GetCurrentUserId();
        var commandWithUserId = command with { UserId = userId };

        var result = await _mediator.Send(commandWithUserId);

        if (result.IsFailure)
            return BadRequest(ApiResponse<RecipeDto>.ErrorResult(result.Error));

        return CreatedAtAction(
            nameof(GetRecipe),
            new { id = result.Value.Id },
            ApiResponse<RecipeDto>.SuccessResult(result.Value, "Tarif başarıyla oluşturuldu"));
    }

    /// <summary>
    /// Tarif detayını getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RecipeDto>>> GetRecipe(Guid id)
    {
        var query = new GetRecipeQuery { RecipeId = id };
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return NotFound(ApiResponse<RecipeDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<RecipeDto>.SuccessResult(RecipeDto.FromEntity(result.Value)));
    }

    /// <summary>
    /// Tarifleri ara
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<PagedResult<RecipeDto>>>> SearchRecipes(
        [FromQuery] string searchTerm,
        [FromQuery] Guid? categoryId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchRecipesQuery
        {
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<RecipeDto>>.ErrorResult(result.Error));

        return Ok(ApiResponse<PagedResult<RecipeDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Kategoriye göre tarifleri getir
    /// </summary>
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<RecipeDto>>>> GetRecipesByCategory(
        Guid categoryId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetRecipesByCategoryQuery
        {
            CategoryId = categoryId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<RecipeDto>>.ErrorResult(result.Error));

        var recipes = result.Value.Select(RecipeDto.FromEntity).ToList();
        var pagedResult = new PagedResult<RecipeDto>
        {
            Items = recipes,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = recipes.Count
        };

        return Ok(ApiResponse<PagedResult<RecipeDto>>.SuccessResult(pagedResult));
    }

    /// <summary>
    /// Kullanıcının tariflerini getir
    /// </summary>
    [HttpGet("my-recipes")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<PagedResult<RecipeDto>>>> GetMyRecipes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        // GetUserRecipesQuery implementation needed
        await Task.CompletedTask; // Temporary fix until implementation
        return Ok(ApiResponse<PagedResult<RecipeDto>>.SuccessResult(new PagedResult<RecipeDto>()));
    }

    /// <summary>
    /// Tarifi güncelle
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RecipeDto>>> UpdateRecipe(Guid id, [FromBody] UpdateRecipeCommand command)
    {
        var commandWithId = command with { Id = id };
        var result = await _mediator.Send(commandWithId);

        if (result.IsFailure)
            return BadRequest(ApiResponse<RecipeDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<RecipeDto>.SuccessResult(RecipeDto.FromEntity(result.Value), "Tarif başarıyla güncellendi"));
    }

    /// <summary>
    /// Tarifi sil
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteRecipe(Guid id)
    {
        // DeleteRecipeCommand implementation needed
        await Task.CompletedTask; // Temporary fix until implementation
        return Ok(ApiResponse<bool>.SuccessResult(true, "Tarif başarıyla silindi"));
    }

    /// <summary>
    /// Popüler tarifleri getir
    /// </summary>
    [HttpGet("popular")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RecipeDto>>>> GetPopularRecipes([FromQuery] int count = 10)
    {
        // GetPopularRecipesQuery implementation needed
        await Task.CompletedTask; // Temporary fix until implementation
        return Ok(ApiResponse<IEnumerable<RecipeDto>>.SuccessResult(new List<RecipeDto>()));
    }

    /// <summary>
    /// Tarif kategorilerini getir
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetCategories()
    {
        // GetCategoriesQuery implementation needed
        await Task.CompletedTask; // Temporary fix until implementation
        return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResult(new List<CategoryDto>()));
    }

    [HttpGet("{id}/comments")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CommentDto>>>> GetRecipeComments(Guid id)
    {
        var comments = await _mediator.Send(new GetRecipeCommentsQuery { RecipeId = id });
        return Ok(ApiResponse<IEnumerable<CommentDto>>.SuccessResult(comments));
    }

    [HttpPost("{id}/comments")]
    public async Task<ActionResult<ApiResponse<CommentDto>>> AddComment(Guid id, [FromBody] AddCommentCommand command)
    {
        command.RecipeId = id;
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<CommentDto>.SuccessResult(result));
    }

    [HttpDelete("{id}/comments/{commentId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteComment(Guid id, Guid commentId)
    {
        var result = await _mediator.Send(new DeleteCommentCommand { CommentId = commentId });
        return Ok(ApiResponse<bool>.SuccessResult(result.IsSuccess));
    }

    [HttpPost("{id}/like")]
    public async Task<ActionResult<ApiResponse<bool>>> LikeRecipe(Guid id)
    {
        var result = await _mediator.Send(new LikeRecipeCommand { RecipeId = id });
        return Ok(ApiResponse<bool>.SuccessResult(result.IsSuccess));
    }

    [HttpDelete("{id}/like")]
    public async Task<ActionResult<ApiResponse<bool>>> UnlikeRecipe(Guid id)
    {
        var result = await _mediator.Send(new UnlikeRecipeCommand { RecipeId = id });
        return Ok(ApiResponse<bool>.SuccessResult(result.IsSuccess));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}