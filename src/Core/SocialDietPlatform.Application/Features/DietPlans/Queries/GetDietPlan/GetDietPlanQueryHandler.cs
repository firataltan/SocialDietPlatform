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

namespace SocialDietPlatform.Application.Features.DietPlans.Queries.GetDietPlan;

public class GetDietPlanQueryHandler : IRequestHandler<GetDietPlanQuery, Result<DietPlanDto>>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    private readonly IMapper _mapper;

    public GetDietPlanQueryHandler(IDietPlanRepository dietPlanRepository, IMapper mapper)
    {
        _dietPlanRepository = dietPlanRepository;
        _mapper = mapper;
    }

    public async Task<Result<DietPlanDto>> Handle(GetDietPlanQuery request, CancellationToken cancellationToken)
    {
        var dietPlan = await _dietPlanRepository.GetDietPlanWithMealsAsync(request.DietPlanId, cancellationToken);
        if (dietPlan == null)
        {
            return Result.Failure<DietPlanDto>("Diyet planı bulunamadı.");
        }

        var dietPlanDto = _mapper.Map<DietPlanDto>(dietPlan);
        return Result.Success(dietPlanDto);
    }
}