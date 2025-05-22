using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface IDietPlanRepository : IBaseRepository<DietPlan>
{
    Task<IEnumerable<DietPlan>> GetUserDietPlansAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<DietPlan?> GetActiveDietPlanAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<DietPlan?> GetDietPlanWithMealsAsync(Guid dietPlanId, CancellationToken cancellationToken = default);
}