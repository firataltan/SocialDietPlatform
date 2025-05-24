using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Features.Posts.Commands.CreatePost;
using SocialDietPlatform.Application.Features.Posts.Commands.LikePost;
using SocialDietPlatform.Application.Features.Posts.Queries.GetFeedPosts;
using SocialDietPlatform.Application.Features.Comments.Commands.AddComment;
using SocialDietPlatform.Application.Features.Comments.Queries.GetPostComments;
using System.Security.Claims;

namespace SocialDietPlatform.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Yeni gönderi oluştur
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PostDto>>> CreatePost([FromBody] CreatePostCommand command)
    {
        var userId = GetCurrentUserId();
        var commandWithUserId = command with { UserId = userId };

        var result = await _mediator.Send(commandWithUserId);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PostDto>.ErrorResult(result.Error));

        return Ok(ApiResponse<PostDto>.SuccessResult(result.Value, "Gönderi başarıyla oluşturuldu"));
    }

    /// <summary>
    /// Feed gönderilerini getir
    /// </summary>
    [HttpGet("feed")]
    public async Task<ActionResult<ApiResponse<PagedResult<PostDto>>>> GetFeedPosts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        var query = new GetFeedPostsQuery { UserId = userId, PageNumber = pageNumber, PageSize = pageSize };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PostDto>>.ErrorResult(result.Error));

        return Ok(ApiResponse<PagedResult<PostDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Gönderiye yorum ekle
    /// </summary>
    [HttpPost("{id}/comments")]
    public async Task<ActionResult<ApiResponse<CommentDto>>> AddComment(Guid id, [FromBody] AddCommentCommand command)
    {
        command.PostId = id;
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<CommentDto>.SuccessResult(result));
    }

    /// <summary>
    /// Gönderinin yorumlarını getir
    /// </summary>
    [HttpGet("{id}/comments")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CommentDto>>>> GetPostComments(Guid id)
    {
        var comments = await _mediator.Send(new GetPostCommentsQuery { PostId = id });
        return Ok(ApiResponse<IEnumerable<CommentDto>>.SuccessResult(comments));
    }

    /// <summary>
    /// Gönderi beğen/beğenmeme
    /// </summary>
    [HttpPost("{postId}/like")]
    public async Task<ActionResult<ApiResponse<bool>>> LikePost(Guid postId)
    {
        var userId = GetCurrentUserId();
        var command = new LikePostCommand { PostId = postId, UserId = userId };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.ErrorResult(result.Error));

        return Ok(ApiResponse<bool>.SuccessResult(result.Value));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}