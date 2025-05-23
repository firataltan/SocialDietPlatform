using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Infrastructure.Configuration;
using SocialDietPlatform.Infrastructure.Services;
using Nest;

namespace SocialDietPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ISearchService, SearchService>();

        // SendGrid
        services.AddSendGrid(options =>
        {
            options.ApiKey = configuration.GetSection("EmailSettings")["ApiKey"]!;
        });

        // Redis Cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        // Elasticsearch
        var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200"))
            .DefaultIndex("socialdiet")
            .EnableDebugMode()
            .PrettyJson();

        services.AddSingleton<IElasticClient>(new ElasticClient(settings));

        return services;
    }
}