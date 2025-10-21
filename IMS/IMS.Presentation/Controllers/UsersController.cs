using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Pagination;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Users.Base)]
public class UsersController(IUserService service, IMapper mapper) : ControllerBase
{   
    [HttpGet]
    public async Task<PagedList<UserDTO>> GetAll(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] UserFilteringParameters filter,
        [FromQuery] UserSortingParameter sorter, 
        CancellationToken cancellationToken)
    {
        var users = await service.GetUsersAsync(paginationParameters, 
            filter, sorter, cancellationToken: cancellationToken);

        var userDTOs = mapper.Map<PagedList<UserDTO>>(users);

        return userDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<UserDTO> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await service.GetByIdAsync(id, cancellationToken);

        var userDTO = mapper.Map<UserDTO>(user);

        return userDTO;
    }

    [HttpGet(ApiRoutes.Users.UsersByRole)]
    public async Task<List<UserDTO>> GetUsersByRole([FromRoute] Role role, CancellationToken cancellationToken)
    {
        var users = await service.GetUsersByRoleAsync(role, cancellationToken: cancellationToken);

        var userDTOs = mapper.Map<List<UserDTO>>(users);

        return userDTOs;
    }

    [HttpPost]
    public async Task<UserDTO> Create([FromBody] CreateUserDTO createUserDTO, CancellationToken cancellationToken)
    {
        var userModel = mapper.Map<UserModel>(createUserDTO);

        var createdUserModel = await service.CreateAsync(userModel, cancellationToken);

        var userDTO = mapper.Map<UserDTO>(createdUserModel);

        return userDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<UserDTO> Update([FromRoute] Guid id, 
        [FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
    {
        var userModel = mapper.Map<UserModel>(updateUserDTO);

        var updatedUserModel = await service.UpdateAsync(id, userModel, cancellationToken);

        var updatedUserDTO = mapper.Map<UserDTO>(updatedUserModel);

        return updatedUserDTO;
    }
}
