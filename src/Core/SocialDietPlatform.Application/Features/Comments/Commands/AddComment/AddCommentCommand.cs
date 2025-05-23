using MediatR;
using Microsoft.AspNetCore.Http;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;
using System.Security.Claims;

namespace SocialDietPlatform.Application.Features.Comments.Commands.AddComment;

public record AddCommentCommand : IRequest<CommentDto>
{
    public Guid RecipeId { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddCommentCommandHandler(
        ICommentRepository commentRepository,
        IRecipeRepository recipeRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _commentRepository = commentRepository;
        _recipeRepository = recipeRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("Kullanıcı girişi yapılmamış.");

        var userId = Guid.Parse(userIdClaim);

        var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe == null)
            throw new KeyNotFoundException("Tarif bulunamadı.");

        var comment = new Comment
        {
            Content = request.Content,
            UserId = userId,
            PostId = request.RecipeId
        };

        await _commentRepository.AddAsync(comment);

        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            UserId = comment.UserId,
            PostId = comment.PostId,
            CreatedAt = comment.CreatedAt
        };
    }
} 