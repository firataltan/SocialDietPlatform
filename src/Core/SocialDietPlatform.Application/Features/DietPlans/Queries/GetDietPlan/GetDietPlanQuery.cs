using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.DietPlans.Queries.GetDietPlan;

public record GetDietPlanQuery : IRequest<Result<DietPlanDto>>
{
    public Guid DietPlanId { get; init; }
}