using MediatR;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;

namespace SocialDietPlatform.Application.Features.Comments.Queries.GetPostComments;

public record GetPostCommentsQuery : IRequest<IEnumerable<CommentDto>>
{
    public Guid PostId { get; init; }
}

public class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentRepository _commentRepository;

    public GetPostCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<CommentDto>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetByPostIdAsync(request.PostId);
        return comments.Select(CommentDto.FromEntity);
    }
} 