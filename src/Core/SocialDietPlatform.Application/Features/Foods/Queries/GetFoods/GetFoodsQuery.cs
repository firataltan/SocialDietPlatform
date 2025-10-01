using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
 
namespace SocialDietPlatform.Application.Features.Foods.Queries.GetFoods
{
    public record GetFoodsQuery : IRequest<Result<IEnumerable<FoodDto>>>;
} 