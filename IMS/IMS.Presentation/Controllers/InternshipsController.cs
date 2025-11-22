using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IMS.Presentation.ApiConstants.Permissions;

namespace IMS.Presentation.Controllers;

[Authorize]
[ApiController]
[Route(ApiRoutes.Internships.Base)]
public class InternshipsController(IInternshipService service, IMapper mapper) : ControllerBase
{
    [Authorize(Internships.Read)]
    [HttpGet]
    public async Task<IEnumerable<InternshipDto>> GetAll(CancellationToken cancellationToken)
    {
        var internships = await service.GetAllAsync(cancellationToken: cancellationToken);

        var internshipDtos = mapper.Map<IEnumerable<InternshipDto>>(internships); 

        return internshipDtos;
    }

    [Authorize(Internships.Read)]
    [HttpGet(ApiRoutes.Id)]
    public async Task<InternshipDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var internship = await service.GetByIdAsync(id, cancellationToken);

        var internshipDto = mapper.Map<InternshipDto>(internship);

        return internshipDto;
    }

    [Authorize(Internships.Create)]
    [HttpPost]
    public async Task<InternshipDto> Create([FromBody] CreateInternshipDto createInternshipDto, 
        CancellationToken cancellationToken)
    {
        var internshipModel = mapper.Map<InternshipModel>(createInternshipDto);

        var createdInternshipModel = await service.CreateInternshipAsync(internshipModel, cancellationToken);

        var internshipDto = mapper.Map<InternshipDto>(createdInternshipModel);

        return internshipDto;
    }

    [Authorize(Internships.Update)]
    [HttpPut(ApiRoutes.Id)]
    public async Task<InternshipDto> Update([FromRoute] Guid id, 
        [FromBody] UpdateInternshipDto updateInternshipDto, CancellationToken cancellationToken)
    {
        var internshipModel = mapper.Map<InternshipModel>(updateInternshipDto);

        var updatedInternshipModel = await service.UpdateAsync(id, internshipModel, cancellationToken);

        var updatedInternshipDto = mapper.Map<InternshipDto>(updatedInternshipModel);

        return updatedInternshipDto;
    }

    [Authorize(Internships.Read)]
    [HttpGet(ApiRoutes.Internships.InternshipsByMentorId)]
    public async Task<List<InternshipDto>> GetInternshipsByMentorId([FromRoute] Guid mentorId,  CancellationToken cancellationToken)
    {
        var internships = await service.GetInternshipsByMentorIdAsync(mentorId, cancellationToken : cancellationToken);

        var internshipDtos = mapper.Map<List<InternshipDto>>(internships);

        return internshipDtos;
    }
}
