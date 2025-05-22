using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Interfaces.Repositories;

public interface IFoodRepository : IBaseRepository<Food>
{
    Task<IEnumerable<Food>> GetByMealIdAsync(Guid mealId, CancellationToken cancellationToken = default);
} 