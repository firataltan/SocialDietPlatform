using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.Features.Posts.Commands.CreatePost;

public record CreatePostCommand : IRequest<Result<PostDto>>
{
    public string Content { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
    public string? VideoUrl { get; init; }
    public PostType Type { get; init; } = PostType.Text;
    public Guid UserId { get; init; }
    public Guid? RecipeId { get; init; }
}