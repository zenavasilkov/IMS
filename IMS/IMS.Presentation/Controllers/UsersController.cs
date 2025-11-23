using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Pagination;
using static IMS.Presentation.ApiConstants.Permissions;

namespace IMS.Presentation.Controllers;

[Authorize]
[ApiController]
[Route(ApiRoutes.Users.Base)]
public class UsersController(IUserService service, IMapper mapper) : ControllerBase
{
    [Authorize(Users.Read)]
    [HttpGet]
    public async Task<PagedList<UserDto>> GetAll(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] UserFilteringParameters filter,
        [FromQuery] UserSortingParameter sorter, 
        CancellationToken cancellationToken)
    {
        var users = await service.GetUsersAsync(paginationParameters, 
            filter, sorter, cancellationToken: cancellationToken);

        var userDtos = mapper.Map<PagedList<UserDto>>(users);

        return userDtos;
    }
    
    [Authorize(Users.Read)]
    [HttpGet(ApiRoutes.Id)]
    public async Task<UserDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await service.GetByIdAsync(id, cancellationToken);

        var userDto = mapper.Map<UserDto>(user);

        return userDto;
    }
    
    [Authorize(Users.Create)]
    [HttpPost]
    public async Task<UserDto> Create([FromBody] CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        var userModel = mapper.Map<UserModel>(createUserDto);

        var createdUserModel = await service.CreateAsync(userModel, cancellationToken);

        var userDto = mapper.Map<UserDto>(createdUserModel);

        return userDto;
    }
    
    [Authorize(Users.Update)]
    [HttpPut(ApiRoutes.Id)]
    public async Task<UserDto> Update([FromRoute] Guid id, 
        [FromBody] UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var userModel = mapper.Map<UserModel>(updateUserDto);

        var updatedUserModel = await service.UpdateAsync(id, userModel, cancellationToken);

        var updatedUserDto = mapper.Map<UserDto>(updatedUserModel);

        return updatedUserDto;
    }
}
