using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialDietPlatform.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(Guid userId, string email, string role);
    Task<bool> ValidateTokenAsync(string token);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}