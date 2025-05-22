using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Persistence.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.HasOne(f => f.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Following)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint - bir kullanıcı aynı kişiyi birden fazla takip edemez
        builder.HasIndex(f => new { f.FollowerId, f.FollowingId })
            .IsUnique();

        // Kendi kendini takip etmeyi engelle
        builder.HasCheckConstraint("CK_Follow_NoSelfFollow",
            "\"FollowerId\" != \"FollowingId\"");
    }
}