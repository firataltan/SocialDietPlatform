using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialDietPlatform.Application.DTOs;

public class MealFoodDto
{
    public Guid Id { get; set; }
    public FoodDto Food { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal Calories { get; set; }
}
