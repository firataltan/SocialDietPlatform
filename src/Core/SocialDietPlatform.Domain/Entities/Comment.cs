using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialDietPlatform.Domain.Common;

namespace SocialDietPlatform.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        // Navigation Properties
        public virtual User? User { get; set; }
        public virtual Post? Post { get; set; }

        public Comment()
        {
            Content = string.Empty;
        }

        public Comment(string content, Guid userId, Guid postId)
        {
            Content = content;
            UserId = userId;
            PostId = postId;
        }
    }
}
