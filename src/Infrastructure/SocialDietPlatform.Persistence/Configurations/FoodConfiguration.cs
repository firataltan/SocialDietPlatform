using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Persistence.Configurations;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Brand)
            .HasMaxLength(100);

        builder.Property(f => f.Barcode)
            .HasMaxLength(50);

        builder.Property(f => f.Unit)
            .IsRequired()
            .HasMaxLength(20);

        // Value Object mapping
        builder.OwnsOne(f => f.NutritionalInfo, n =>
        {
            n.Property(ni => ni.Calories).HasColumnType("decimal(8,2)");
            n.Property(ni => ni.Protein).HasColumnType("decimal(8,2)");
            n.Property(ni => ni.Carbohydrates).HasColumnType("decimal(8,2)");
            n.Property(ni => ni.Fat).HasColumnType("decimal(8,2)");
            n.Property(ni => ni.Fiber).HasColumnType("decimal(8,2)");
            n.Property(ni => ni.Sugar).HasColumnType("decimal(8,2)");
        });

        builder.HasIndex(f => f.Name);
        builder.HasIndex(f => f.Barcode).IsUnique();
    }
}