using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.DietPlans.Queries.GetUserDietPlans;

public record GetUserDietPlansQuery : IRequest<Result<IEnumerable<DietPlanDto>>>
{
    public Guid UserId { get; init; }
}