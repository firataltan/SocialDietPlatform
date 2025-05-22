using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;

namespace SocialDietPlatform.Application.Features.Posts.Queries.GetFeedPosts;

public class GetFeedPostsQueryHandler : IRequestHandler<GetFeedPostsQuery, Result<PagedResult<PostDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetFeedPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<PostDto>>> Handle(GetFeedPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetFeedPostsAsync(request.UserId, request.PageNumber, request.PageSize, cancellationToken);
        var totalCount = await _postRepository.CountAsync(cancellationToken: cancellationToken);

        var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);

        var result = new PagedResult<PostDto>
        {
            Items = postDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return Result.Success(result);
    }
}