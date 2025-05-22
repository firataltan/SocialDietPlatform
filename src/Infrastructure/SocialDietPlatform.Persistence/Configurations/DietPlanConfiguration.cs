using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Persistence.Configurations;

public class DietPlanConfiguration : IEntityTypeConfiguration<DietPlan>
{
    public void Configure(EntityTypeBuilder<DietPlan> builder)
    {
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.TargetCalories)
            .HasColumnType("decimal(8,2)");

        builder.HasOne(d => d.User)
            .WithMany(u => u.DietPlans)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Dietitian)
            .WithMany()
            .HasForeignKey(d => d.DietitianId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.Meals)
            .WithOne(m => m.DietPlan)
            .HasForeignKey(m => m.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => d.UserId);
        builder.HasIndex(d => d.StartDate);
        builder.HasIndex(d => d.EndDate);
    }
}