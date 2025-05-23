using MediatR;
using Microsoft.AspNetCore.Http;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Repositories;
using System.Security.Claims;

namespace SocialDietPlatform.Application.Features.Comments.Commands.DeleteComment;

public record DeleteCommentCommand : IRequest<Result>
{
    public Guid CommentId { get; init; }
}

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteCommentCommandHandler(
        ICommentRepository commentRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _commentRepository = commentRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Result.Failure("Kullanıcı girişi yapılmamış.");

        var userId = Guid.Parse(userIdClaim);
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);

        if (comment == null)
            return Result.Failure("Yorum bulunamadı.");

        if (comment.UserId != userId)
            return Result.Failure("Bu yorumu silme yetkiniz yok.");

        await _commentRepository.DeleteAsync(comment);
        return Result.Success();
    }
} 