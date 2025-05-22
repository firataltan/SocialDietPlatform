using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using SocialDietPlatform.Application.Common.Models;
using SocialDietPlatform.Application.DTOs;
using SocialDietPlatform.Application.Interfaces.Repositories;

namespace SocialDietPlatform.Application.Features.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, Result<PagedResult<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SearchUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<UserDto>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.SearchUsersAsync(
            request.SearchTerm,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var totalCount = await _userRepository.CountAsync(
            u => u.FirstName.Contains(request.SearchTerm) ||
                 u.LastName.Contains(request.SearchTerm) ||
                 u.UserName!.Contains(request.SearchTerm),
            cancellationToken);

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

        var result = new PagedResult<UserDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return Result.Success(result);
    }
}