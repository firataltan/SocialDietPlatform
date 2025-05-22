using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialDietPlatform.Domain.ValueObjects;

public record NutritionalInfo
{
    public decimal Calories { get; }
    public decimal Protein { get; }
    public decimal Carbohydrates { get; }
    public decimal Fat { get; }
    public decimal Fiber { get; }
    public decimal Sugar { get; }

    public NutritionalInfo(decimal calories, decimal protein, decimal carbohydrates, decimal fat, decimal fiber, decimal sugar)
    {
        if (calories < 0) throw new ArgumentException("Calories cannot be negative");
        if (protein < 0) throw new ArgumentException("Protein cannot be negative");
        if (carbohydrates < 0) throw new ArgumentException("Carbohydrates cannot be negative");
        if (fat < 0) throw new ArgumentException("Fat cannot be negative");
        if (fiber < 0) throw new ArgumentException("Fiber cannot be negative");
        if (sugar < 0) throw new ArgumentException("Sugar cannot be negative");

        Calories = calories;
        Protein = protein;
        Carbohydrates = carbohydrates;
        Fat = fat;
        Fiber = fiber;
        Sugar = sugar;
    }
}