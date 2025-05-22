using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Posts.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISearchService _searchService;

    public CreatePostCommandHandler(
        IPostRepository postRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISearchService searchService)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<PostDto>("Kullanıcı bulunamadı.");
        }

        var post = new Post
        {
            Content = request.Content,
            ImageUrl = request.ImageUrl,
            VideoUrl = request.VideoUrl,
            Type = request.Type,
            UserId = request.UserId,
            RecipeId = request.RecipeId
        };

        await _postRepository.AddAsync(post, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Elasticsearch'e indexle
        await _searchService.IndexDocumentAsync(post, "posts", cancellationToken);

        var postDto = _mapper.Map<PostDto>(post);
        postDto.User = _mapper.Map<UserDto>(user);

        return Result.Success(postDto);
    }
}
