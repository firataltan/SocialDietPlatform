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

namespace SocialDietPlatform.Application.Features.Posts.Commands.UpdatePost;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
        {
            return Result.Failure<PostDto>("Gönderi bulunamadı.");
        }

        if (post.UserId != request.UserId)
        {
            return Result.Failure<PostDto>("Bu gönderiyi düzenleme yetkiniz yok.");
        }

        post.Content = request.Content;
        post.ImageUrl = request.ImageUrl;
        post.VideoUrl = request.VideoUrl;
        post.UpdatedAt = DateTime.UtcNow;

        _postRepository.Update(post);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var postDto = _mapper.Map<PostDto>(post);
        return Result.Success(postDto);
    }
}