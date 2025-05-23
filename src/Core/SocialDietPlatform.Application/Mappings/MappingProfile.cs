﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Domain.Entities;

namespace SocialDietPlatform.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.BMI, opt => opt.MapFrom(src =>
                src.Weight.HasValue && src.Height.HasValue && src.Height > 0
                ? src.Weight / (src.Height * src.Height / 10000)
                : null));

        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count));

        CreateMap<Comment, CommentDto>();
        CreateMap<DietPlan, DietPlanDto>();
        CreateMap<Meal, MealDto>();
        CreateMap<Food, FoodDto>();
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.PreparationTime, opt => opt.MapFrom(src => src.PreparationTime))
            .ForMember(dest => dest.CookingTime, opt => opt.MapFrom(src => src.CookingTime))
            .ForMember(dest => dest.TotalTimeMinutes, opt => opt.MapFrom(src => src.TotalTimeMinutes))
            .ForMember(dest => dest.CaloriesPerServing, opt => opt.MapFrom(src => src.CaloriesPerServing))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count));

        CreateMap<RecipeIngredient, RecipeIngredientDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Notification, NotificationDto>();
        CreateMap<MealFood, MealFoodDto>();

        CreateMap<Domain.ValueObjects.NutritionalInfo, NutritionalInfoDto>().ReverseMap();
    }
}