using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;

namespace SocialDietPlatform.Application.Features.Recipes.Queries.SearchRecipes;

public record SearchRecipesQuery : IRequest<Result<PagedResult<RecipeDto>>>
{
    public string SearchTerm { get; init; } = string.Empty;
    public Guid? CategoryId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}