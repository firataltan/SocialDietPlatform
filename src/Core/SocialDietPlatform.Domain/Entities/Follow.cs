using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;

namespace SocialDietPlatform.Domain.Entities;

public class Follow : BaseEntity
{
    public Guid FollowerId { get; set; }
    public Guid FollowingId { get; set; }

    // Navigation Properties
    public virtual User Follower { get; set; } = null!;
    public virtual User Following { get; set; } = null!;
}