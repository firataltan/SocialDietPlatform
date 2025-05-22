using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces;
using SocialDietPlatform.Application.Interfaces.Repositories;

namespace SocialDietPlatform.Application.Features.Posts.Commands.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Result<bool>>
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
        {
            return Result.Failure<bool>("Gönderi bulunamadı.");
        }

        if (post.UserId != request.UserId)
        {
            return Result.Failure<bool>("Bu gönderiyi silme yetkiniz yok.");
        }

        _postRepository.Delete(post);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}