using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces;
using SocialDietPlatform.Application.Interfaces.Repositories;

namespace SocialDietPlatform.Application.Features.DietPlans.Commands.UpdateDietPlan;

public class UpdateDietPlanCommandHandler : IRequestHandler<UpdateDietPlanCommand, Result<DietPlanDto>>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDietPlanCommandHandler(IDietPlanRepository dietPlanRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _dietPlanRepository = dietPlanRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DietPlanDto>> Handle(UpdateDietPlanCommand request, CancellationToken cancellationToken)
    {
        // Business rule validation
        // var dateValidationRule = new DietPlanDateValidationRule(request.StartDate, request.EndDate);
        // dateValidationRule.CheckRule();

        var dietPlan = await _dietPlanRepository.GetByIdAsync(request.DietPlanId, cancellationToken);
        if (dietPlan == null)
        {
            return Result.Failure<DietPlanDto>("Diyet planı bulunamadı.");
        }

        dietPlan.Name = request.Name;
        dietPlan.Description = request.Description;
        dietPlan.StartDate = request.StartDate;
        dietPlan.EndDate = request.EndDate;
        dietPlan.TargetCalories = request.TargetCalories;
        dietPlan.UpdatedAt = DateTime.UtcNow;

        // Save changes
        _dietPlanRepository.Update(dietPlan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return mapped DTO
        var dietPlanDto = _mapper.Map<DietPlanDto>(dietPlan);
        return Result.Success(dietPlanDto);
    }
}
