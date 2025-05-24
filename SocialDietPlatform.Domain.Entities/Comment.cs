using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocialDietPlatform.Domain.Entities.Base;
using SocialDietPlatform.Domain.Entities.Users;
using SocialDietPlatform.Domain.Entities.Posts;
using SocialDietPlatform.Domain.Entities.Recipes;

namespace SocialDietPlatform.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public Guid? RecipeId { get; set; }

        // Navigation Properties
        public virtual User? User { get; set; }
        public virtual Post? Post { get; set; }
        // public virtual Recipe? Recipe { get; set; } // Optional navigation property
    }
} 