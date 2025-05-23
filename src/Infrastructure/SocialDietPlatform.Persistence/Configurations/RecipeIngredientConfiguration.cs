using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Persistence.Configurations;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.HasKey(ri => new { ri.RecipeId, ri.FoodId });

        builder.Property(ri => ri.Quantity)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(ri => ri.Calories)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ri => ri.Food)
            .WithMany(f => f.RecipeIngredients)
            .HasForeignKey(ri => ri.FoodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 