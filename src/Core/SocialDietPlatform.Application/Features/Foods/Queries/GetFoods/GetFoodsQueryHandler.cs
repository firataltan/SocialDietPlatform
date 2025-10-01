using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Features.Foods.Queries.GetFoods
{
    public class GetFoodsQueryHandler : IRequestHandler<GetFoodsQuery, Result<IEnumerable<FoodDto>>>
    {
        private readonly IFoodRepository _foodRepository;

        public GetFoodsQueryHandler(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task<Result<IEnumerable<FoodDto>>> Handle(GetFoodsQuery request, CancellationToken cancellationToken)
        {
            var foods = await _foodRepository.GetAllAsync();

            var foodDtos = foods.Select(f => new FoodDto
            {
                Id = f.Id,
                Name = f.Name,
                Brand = f.Brand,
                Barcode = f.Barcode,
                Calories = f.NutritionalInfo.Calories,
                Protein = f.NutritionalInfo.Protein,
                Carbs = f.NutritionalInfo.Carbohydrates,
                Fat = f.NutritionalInfo.Fat,
                Unit = f.Unit
            }).ToList();

            return Result<IEnumerable<FoodDto>>.Success(foodDtos);
        }
    }
} 