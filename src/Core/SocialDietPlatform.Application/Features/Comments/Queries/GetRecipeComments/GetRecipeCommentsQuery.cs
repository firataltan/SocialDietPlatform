using MediatR;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Comments.Queries.GetRecipeComments;

public record GetRecipeCommentsQuery : IRequest<IEnumerable<CommentDto>>
{
    public Guid RecipeId { get; init; }
}

public class GetRecipeCommentsQueryHandler : IRequestHandler<GetRecipeCommentsQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentRepository _commentRepository;

    public GetRecipeCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<CommentDto>> Handle(GetRecipeCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetByPostIdAsync(request.RecipeId);
        return comments.Select(CommentDto.FromEntity);
    }
} 