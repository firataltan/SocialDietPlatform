using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Domain.Entities;

public class Post : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public PostType Type { get; set; } = PostType.Text;
    public Guid UserId { get; set; }
    public Guid? RecipeId { get; set; }

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual Recipe? Recipe { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public int LikeCount => Likes.Count;
    public int CommentCount => Comments.Count;
}