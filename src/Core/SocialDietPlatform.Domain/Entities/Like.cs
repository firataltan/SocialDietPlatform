using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;

namespace SocialDietPlatform.Domain.Entities;

public class Like : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual Post Post { get; set; } = null!;
}
