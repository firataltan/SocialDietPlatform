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

namespace SocialDietPlatform.Application.Features.Posts.Queries.GetPostById;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Result<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostByIdQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostWithDetailsAsync(request.PostId, cancellationToken);
        if (post == null)
        {
            return Result.Failure<PostDto>("Gönderi bulunamadı.");
        }

        var postDto = _mapper.Map<PostDto>(post);

        // Check if current user liked this post
        if (request.CurrentUserId.HasValue)
        {
            postDto.IsLiked = post.Likes.Any(l => l.UserId == request.CurrentUserId.Value);
        }

        return Result.Success(postDto);
    }
}
