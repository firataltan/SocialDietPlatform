using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        builder.Property(p => p.VideoUrl)
            .HasMaxLength(500);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict); // CASCADE yerine RESTRICT

        //builder.HasOne(p => p.Recipe)
        //    .WithMany(r => r.Posts)
        //    .HasForeignKey(p => p.RecipeId)
        //    .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade); // Post silinince comment'ler silinebilir

        builder.HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade); // Post silinince like'lar silinebilir

        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => p.Type);
    }
}