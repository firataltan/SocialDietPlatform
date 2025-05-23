namespace SocialDietPlatform.Infrastructure.Configuration;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationInMinutes { get; set; }

    public JwtSettings()
    {
        SecretKey = string.Empty;
        Issuer = string.Empty;
        Audience = string.Empty;
    }
} 