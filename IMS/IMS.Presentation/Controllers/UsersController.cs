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
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllAsync(null, false, cancellationToken);

        var response = _mapper.Map<IEnumerable<UserDTO>>(users);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _service.GetByIdAsync(id, cancellationToken);

        if (user is null) return NotFound(new { message = $"User with ID {id} was not found." });

        var userDTO = _mapper.Map<UserDTO>(user);

        return Ok(userDTO);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create([FromBody] CreateUserDTO createUserDTO, CancellationToken cancellationToken)
    {
        var userModel = _mapper.Map<UserModel>(createUserDTO);
        var createdUserModel = await _service.CreateAsync(userModel, cancellationToken);
        var userDTO = _mapper.Map<UserDTO>(createdUserModel);
        return CreatedAtAction(nameof(GetById), new { id = userDTO.Id }, userDTO);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDTO>> Update(Guid id, [FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
    {
        var userModel = _mapper.Map<UserModel>(updateUserDTO);

        userModel.Id = id;

        var updatedUserModel = await _service.UpdateAsync(userModel, cancellationToken);

        if (updatedUserModel is null) return NotFound(new { message = $"User with ID {id} was not found." });

        var updatedUserDTO = _mapper.Map<UserDTO>(updatedUserModel);

        return Ok(updatedUserDTO);
    }

    [HttpPatch("mentor/{mentorId:guid}/intern/{internId:guid}")]
    public async Task<ActionResult<UserDTO>> AddInternToMentorById(Guid mentorId, Guid internId, CancellationToken cancellationToken)
    {
        var mentorModel = await _service.GetMentorByIdAsync(mentorId, cancellationToken);

        if (mentorModel is null)
        {
            return NotFound(new { message = $"Mentor with ID {mentorId} was not found" });
        }

        var internModel = await _service.GetInternByIdAsync(internId, cancellationToken);

        if (internModel is null)
        {
            return NotFound(new { message = $"Intern with ID {internId} was not found" });
        }

        mentorModel.Interns ??= [];

        mentorModel.Interns.Add(internModel);

        var updatedMentorModel = await _service.UpdateAsync(mentorModel, cancellationToken);

        var updatedMentorDTO = _mapper.Map<UserDTO>(updatedMentorModel);

        return Ok(updatedMentorDTO);
    }
}
