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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InternshipDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var internships = await service.GetAllAsync(null, false, cancellationToken);

        var internshipDTOs = mapper.Map<IEnumerable<InternshipDTO>>(internships);

        if (!internshipDTOs.Any()) return NoContent();

        return Ok(internshipDTOs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InternshipDTO>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var internship = await service.GetByIdAsync(id, cancellationToken);

        if (internship is null) return NotFound(new { message = $"Internship with ID {id} was not found." });

        var internshipDTO = mapper.Map<InternshipDTO>(internship);

        return Ok(internshipDTO);
    }

    [HttpPost]
    public async Task<ActionResult<InternshipDTO>> Create([FromBody] CreateInternshipDTO createInternshipDTO, CancellationToken cancellationToken)
    {
        var internshipModel = mapper.Map<InternshipModel>(createInternshipDTO);

        var createdInternshipModel = await service.CreateAsync(internshipModel, cancellationToken);

        var internshipDTO = mapper.Map<InternshipDTO>(createdInternshipModel);

        return CreatedAtAction(nameof(GetById), new { id = internshipDTO.Id }, internshipDTO);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<InternshipDTO>> Update(Guid id, [FromBody] UpdateInternshipDTO updateInternshipDTO, CancellationToken cancellationToken)
    {
        var internshipModel = mapper.Map<InternshipModel>(updateInternshipDTO);

        internshipModel.Id = id;

        var updatedInternshipModel = await service.UpdateAsync(internshipModel, cancellationToken);

        if (updatedInternshipModel is null) return NotFound(new { message = $"Internship with ID {id} was not found." });

        var updatedInternshipDTO = mapper.Map<InternshipDTO>(updatedInternshipModel);

        return Ok(updatedInternshipDTO);
    }
}
