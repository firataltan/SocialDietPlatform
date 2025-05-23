using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Persistence.Configurations;

public class MealFoodConfiguration : IEntityTypeConfiguration<MealFood>
{
    public void Configure(EntityTypeBuilder<MealFood> builder)
    {
        builder.HasKey(mf => new { mf.MealId, mf.FoodId });

        builder.Property(mf => mf.Quantity)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(mf => mf.Calories)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.HasOne(mf => mf.Meal)
            .WithMany(m => m.MealFoods)
            .HasForeignKey(mf => mf.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mf => mf.Food)
            .WithMany(f => f.MealFoods)
            .HasForeignKey(mf => mf.FoodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 