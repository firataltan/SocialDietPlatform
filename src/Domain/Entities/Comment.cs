using System;

namespace Domain.Entities
{
    public class Comment
    {
        public Guid RecipeId { get; set; }

        // Navigation Properties
        public virtual User? User { get; set; }
        public virtual Post? Post { get; set; }
        public virtual Recipe? Recipe { get; set; }

        public Comment()
        {
            Content = string.Empty;
        }

        public Comment(string content, Guid userId, Guid postId)
        {
            // ... existing code ...
        }
    }
} 