using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Identity;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAuthService _authService;

    public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IAuthService authService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailOrUsername) ??
                   await _userManager.FindByNameAsync(request.EmailOrUsername);

        if (user == null)
        {
            return Result.Failure<LoginResponse>("Kullanıcı bulunamadı.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return Result.Failure<LoginResponse>("Hesabınız geçici olarak kilitlendi.");

            return Result.Failure<LoginResponse>("E-posta/kullanıcı adı veya şifre hatalı.");
        }

        var token = await _authService.GenerateJwtTokenAsync(user.Id, user.Email!, user.Role.ToString());

        var response = new LoginResponse
        {
            AccessToken = token,
            RefreshToken = Guid.NewGuid().ToString(), // Implement proper refresh token logic
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };

        return Result.Success(response);
    }
}
