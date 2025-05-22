using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;

namespace SocialDietPlatform.Application.Features.DietPlans.Queries.GetUserDietPlans;

public class GetUserDietPlansQueryHandler : IRequestHandler<GetUserDietPlansQuery, Result<IEnumerable<DietPlanDto>>>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    private readonly IMapper _mapper;

    public GetUserDietPlansQueryHandler(IDietPlanRepository dietPlanRepository, IMapper mapper)
    {
        _dietPlanRepository = dietPlanRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<DietPlanDto>>> Handle(GetUserDietPlansQuery request, CancellationToken cancellationToken)
    {
        var dietPlans = await _dietPlanRepository.GetUserDietPlansAsync(request.UserId, cancellationToken);
        var dietPlanDtos = _mapper.Map<IEnumerable<DietPlanDto>>(dietPlans);

        return Result.Success(dietPlanDtos);
    }
}
