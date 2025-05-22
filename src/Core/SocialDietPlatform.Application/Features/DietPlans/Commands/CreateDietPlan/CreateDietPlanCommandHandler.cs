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
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Domain.Entities;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Application.Features.DietPlans.Commands.CreateDietPlan;

public class CreateDietPlanCommandHandler : IRequestHandler<CreateDietPlanCommand, Result<DietPlanDto>>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public CreateDietPlanCommandHandler(
        IDietPlanRepository dietPlanRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        INotificationService notificationService)
    {
        _dietPlanRepository = dietPlanRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<Result<DietPlanDto>> Handle(CreateDietPlanCommand request, CancellationToken cancellationToken)
    {
        // Business rule validation
        // var dateValidationRule = new DietPlanDateValidationRule(request.StartDate, request.EndDate);
        // dateValidationRule.CheckRule();

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<DietPlanDto>("Kullanıcı bulunamadı.");
        }

        if (request.DietitianId.HasValue)
        {
            var dietitian = await _userRepository.GetByIdAsync(request.DietitianId.Value, cancellationToken);
            if (dietitian == null || dietitian.Role != Domain.Enums.UserRole.Dietitian)
            {
                return Result.Failure<DietPlanDto>("Diyetisyen bulunamadı.");
            }
        }

        var dietPlan = new DietPlan
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TargetCalories = request.TargetCalories,
            UserId = request.UserId,
            DietitianId = request.DietitianId
        };

        await _dietPlanRepository.AddAsync(dietPlan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Send notification if assigned by dietitian
        if (request.DietitianId.HasValue)
        {
            var dietitian = await _userRepository.GetByIdAsync(request.DietitianId.Value, cancellationToken);
            await _notificationService.SendNotificationAsync(
                request.UserId,
                "Yeni Diyet Planı",
                $"{dietitian!.FullName} size yeni bir diyet planı atadı: {request.Name}",
                NotificationType.DietPlanAssigned,
                dietPlan.Id,
                "DietPlan",
                cancellationToken);
        }

        var dietPlanDto = _mapper.Map<DietPlanDto>(dietPlan);
        return Result.Success(dietPlanDto);
    }
}