using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.DTOs;

public class PostDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public PostType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserDto User { get; set; } = null!;
    public RecipeDto? Recipe { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool IsLiked { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}