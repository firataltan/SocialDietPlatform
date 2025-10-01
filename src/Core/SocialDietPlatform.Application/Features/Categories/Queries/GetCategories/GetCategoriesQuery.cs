using MediatR;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Common.Models;
 
namespace SocialDietPlatform.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<Result<IEnumerable<CategoryDto>>>;
} 