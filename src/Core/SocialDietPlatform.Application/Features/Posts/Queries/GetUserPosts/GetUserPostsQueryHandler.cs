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

namespace SocialDietPlatform.Application.Features.Posts.Queries.GetUserPosts;

public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, Result<PagedResult<PostDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetUserPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<PostDto>>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetUserPostsAsync(
            request.UserId,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var totalCount = await _postRepository.CountAsync(
            p => p.UserId == request.UserId,
            cancellationToken);

        var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);

        // Set IsLiked property for current user
        if (request.CurrentUserId.HasValue)
        {
            foreach (var postDto in postDtos)
            {
                var post = posts.First(p => p.Id == postDto.Id);
                postDto.IsLiked = post.Likes.Any(l => l.UserId == request.CurrentUserId.Value);
            }
        }

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