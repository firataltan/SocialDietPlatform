using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace SocialDietPlatform.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            NotFoundException => new ApiResponse<object>
            {
                Success = false,
                Message = exception.Message,
                Errors = new List<string> { exception.Message }
            },
            ValidationException => new ApiResponse<object>
            {
                Success = false,
                Message = "Doğrulama hatası",
                Errors = new List<string> { exception.Message }
            },
            DomainException => new ApiResponse<object>
            {
                Success = false,
                Message = exception.Message,
                Errors = new List<string> { exception.Message }
            },
            _ => new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata oluştu",
                Errors = new List<string> { "İç sunucu hatası" }
            }
        };

        context.Response.StatusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            ValidationException => (int)HttpStatusCode.BadRequest,
            DomainException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
