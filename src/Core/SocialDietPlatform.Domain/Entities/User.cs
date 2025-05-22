using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using SocialDietPlatform.Domain.Common;
using SocialDietPlatform.Domain.Enums;
using SocialDietPlatform.Domain.ValueObjects;

namespace SocialDietPlatform.Domain.Entities;

public class User : IdentityUser<Guid>, IAuditable, ISoftDelete
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public decimal? TargetWeight { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    // Navigation Properties
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public virtual ICollection<Follow> Following { get; set; } = new List<Follow>();
    public virtual ICollection<DietPlan> DietPlans { get; set; } = new List<DietPlan>();
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<Appointment> ClientAppointments { get; set; } = new List<Appointment>();
    public virtual ICollection<Appointment> DietitianAppointments { get; set; } = new List<Appointment>();

    public string FullName => $"{FirstName} {LastName}";
    public decimal? BMI => Weight.HasValue && Height.HasValue && Height > 0
        ? Weight / (Height * Height / 10000)
        : null;
}