using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.DietPlans.Commands.CreateDietPlan;

public record CreateDietPlanCommand : IRequest<Result<DietPlanDto>>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal TargetCalories { get; init; }
    public Guid UserId { get; init; }
    public Guid? DietitianId { get; init; }
}
