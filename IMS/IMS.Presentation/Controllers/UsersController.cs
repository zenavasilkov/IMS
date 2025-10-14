using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Users.Base)]
public class UsersController(IUserService service, IMapper mapper) : ControllerBase
{   
    [HttpGet]
    public async Task<IEnumerable<UserDTO>> GetAll(CancellationToken cancellationToken)
    {
        var users = await service.GetAllAsync(null, false, cancellationToken); 

        if (users.Count == 0) throw new Exception("No users have been found");

        var userDTOs = mapper.Map<IEnumerable<UserDTO>>(users);

        return userDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<UserDTO> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await service.GetByIdAsync(id, cancellationToken) ?? throw new Exception($"User with ID {id} was not found.");

        var userDTO = mapper.Map<UserDTO>(user);

        return userDTO;
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
    public async Task<UserDTO> Update(Guid id, [FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
    {
        var userModel = mapper.Map<UserModel>(updateUserDTO);

        userModel.Id = id;

        var updatedUserModel = await service.UpdateAsync(userModel, cancellationToken) ?? throw new Exception($"User with ID {id} was not found.");

        var updatedUserDTO = mapper.Map<UserDTO>(updatedUserModel);

        return updatedUserDTO;
    }

    [HttpPatch(ApiRoutes.Users.AddInternToMentor)]
    public async Task<UserDTO> AddInternToMentorById(Guid mentorId, Guid internId, CancellationToken cancellationToken)
    { 
        var updatedMentorModel = await service.AddInternToMentorById(mentorId, internId, cancellationToken) 
            ?? throw new Exception($"Mentor or intern was not found.");

        var updatedMentorDTO = mapper.Map<UserDTO>(updatedMentorModel);

        return updatedMentorDTO;
    }
}
