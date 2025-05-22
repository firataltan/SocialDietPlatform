using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.DTOs;

public class MealDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MealType Type { get; set; }
    public DateTime PlannedTime { get; set; }
    public DateTime? ConsumedTime { get; set; }
    public bool IsConsumed { get; set; }
    public decimal TotalCalories { get; set; }
    public List<MealFoodDto> Foods { get; set; } = new();
}