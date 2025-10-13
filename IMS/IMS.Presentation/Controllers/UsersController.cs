using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service, IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _service = service;

    [HttpGet]
    public async Task<IEnumerable<UserDTO>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllAsync(null, false, cancellationToken);
        var response = _mapper.Map<IEnumerable<UserDTO>>(users);
        return response;
    }

    [HttpGet("{id:guid}")]
    public async Task<UserDTO> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _service.GetByIdAsync(id, cancellationToken);
        var userDTO = _mapper.Map<UserDTO>(user);
        return userDTO;
    }

    [HttpPost]
    public async Task<UserDTO> Create([FromBody] CreateUserDTO createUserDTO, CancellationToken cancellationToken)
    {
        var userModel = _mapper.Map<UserModel>(createUserDTO);
        var createdUserModel = await _service.CreateAsync(userModel, cancellationToken);
        var userDTO = _mapper.Map<UserDTO>(createdUserModel);
        return userDTO;
    }

    [HttpPut]
    public async Task<UserDTO> Update(Guid id, [FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
    {
        var userModel = _mapper.Map<UserModel>(updateUserDTO);
        userModel.Id = id;
        var updatedUserModel = await _service.UpdateAsync(userModel, cancellationToken);
        var updatedUSerDTO = _mapper.Map<UserDTO>(updatedUserModel);
        return updatedUSerDTO;
    }

    [HttpPatch]
    public async Task<IActionResult> AddInternToMentorById(Guid mentorId, Guid internId, CancellationToken cancellationToken)
    {
        var mentorModel = await _service.GetMentorByIdAsync(mentorId, cancellationToken);

        if (mentorModel is null)
        {
            return BadRequest(new { message = $"Mentor with id {mentorId} not found" });
        }

        var internModel = await _service.GetInternByIdAsync(internId, cancellationToken);

        if (internModel is null)
        {
            return BadRequest(new { message = $"Intern with id {internId} not found" });
        }

        mentorModel.Interns.Add(internModel);

        await _service.UpdateAsync(mentorModel, cancellationToken);

        return Ok("Intern have been successefully added to mentor");
    }
}
