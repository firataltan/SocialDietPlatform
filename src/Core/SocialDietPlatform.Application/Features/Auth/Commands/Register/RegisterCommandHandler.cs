using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public RegisterCommandHandler(UserManager<User> userManager, IMapper mapper, IEmailService emailService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result.Failure<UserDto>("Bu e-posta adresi zaten kullanılıyor.");
        }

        existingUser = await _userManager.FindByNameAsync(request.UserName);
        if (existingUser != null)
        {
            return Result.Failure<UserDto>("Bu kullanıcı adı zaten kullanılıyor.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName,
            DateOfBirth = request.DateOfBirth,
            Role = request.Role,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure<UserDto>($"Kullanıcı oluşturulamadı: {errors}");
        }

        // E-posta doğrulama gönder
        await _emailService.SendWelcomeEmailAsync(user.Email!, user.FirstName, cancellationToken);

        var userDto = _mapper.Map<UserDto>(user);
        return Result.Success(userDto);
    }
}