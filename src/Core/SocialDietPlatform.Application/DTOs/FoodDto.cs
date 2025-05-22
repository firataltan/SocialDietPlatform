using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialDietPlatform.Application.DTOs;

public class FoodDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public NutritionalInfoDto NutritionalInfo { get; set; } = null!;
    public string Unit { get; set; } = string.Empty;
}