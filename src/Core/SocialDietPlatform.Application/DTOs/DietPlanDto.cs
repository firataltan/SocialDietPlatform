using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialDietPlatform.Application.DTOs;

public class DietPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TargetCalories { get; set; }
    public bool IsActive { get; set; }
    public UserDto? Dietitian { get; set; }
    public List<MealDto> Meals { get; set; } = new();
}