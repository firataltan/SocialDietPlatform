using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface IMealRepository : IBaseRepository<Meal>
{
    Task<IEnumerable<Meal>> GetByDietPlanIdAsync(Guid dietPlanId, CancellationToken cancellationToken = default);
} 