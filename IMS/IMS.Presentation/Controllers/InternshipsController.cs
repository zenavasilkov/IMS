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
public class InternshipsController(IInternshipService service, IMapper mapper) : ControllerBase
{
    private readonly IInternshipService _service = service; 
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InternshipDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var internships = await _service.GetAllAsync(null, false, cancellationToken);

        var internshipDTOs = _mapper.Map<IEnumerable<InternshipDTO>>(internships);

        if (!internshipDTOs.Any()) return NoContent();

        return Ok(internshipDTOs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InternshipDTO>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var internship = await _service.GetByIdAsync(id, cancellationToken);

        if (internship is null) return NotFound(new { message = $"Internship with ID {id} was not found." });

        var internshipDTO = _mapper.Map<InternshipDTO>(internship);

        return Ok(internshipDTO);
    }

    [HttpPost]
    public async Task<ActionResult<InternshipDTO>> Create([FromBody] CreateInternshipDTO createInternshipDTO, CancellationToken cancellationToken)
    {
        var internshipModel = _mapper.Map<InternshipModel>(createInternshipDTO);

        var createdInternshipModel = await _service.CreateAsync(internshipModel, cancellationToken);

        var internshipDTO = _mapper.Map<InternshipDTO>(createdInternshipModel);

        return CreatedAtAction(nameof(GetById), new { id = internshipDTO.Id }, internshipDTO);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<InternshipDTO>> Update(Guid id, [FromBody] UpdateInternshipDTO updateInternshipDTO, CancellationToken cancellationToken)
    {
        var internshipModel = _mapper.Map<InternshipModel>(updateInternshipDTO);

        internshipModel.Id = id;

        var updatedInternshipModel = await _service.UpdateAsync(internshipModel, cancellationToken);

        if (updatedInternshipModel is null) return NotFound(new { message = $"Internship with ID {id} was not found." });

        var updatedInternshipDTO = _mapper.Map<InternshipDTO>(updatedInternshipModel);

        return Ok(updatedInternshipDTO);
    }
}
