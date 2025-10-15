using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Internships.Base)]
public class InternshipsController(IService<InternshipModel, Internship> service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<InternshipDTO>> GetAll(CancellationToken cancellationToken)
    {
        var internships = await service.GetAllAsync(null, false, cancellationToken);

        var internshipDTOs = mapper.Map<IEnumerable<InternshipDTO>>(internships); 

        return internshipDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<InternshipDTO> GetById(Guid id, CancellationToken cancellationToken)
    {
        var internship = await service.GetByIdAsync(id, cancellationToken);

        var internshipDTO = mapper.Map<InternshipDTO>(internship);

        return internshipDTO;
    }

    [HttpPost]
    public async Task<InternshipDTO> Create([FromBody] CreateInternshipDTO createInternshipDTO, CancellationToken cancellationToken)
    {
        var internshipModel = mapper.Map<InternshipModel>(createInternshipDTO);

        var createdInternshipModel = await service.CreateAsync(internshipModel, cancellationToken);

        var internshipDTO = mapper.Map<InternshipDTO>(createdInternshipModel);

        return internshipDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<InternshipDTO> Update(Guid id, [FromBody] UpdateInternshipDTO updateInternshipDTO, CancellationToken cancellationToken)
    {
        var internshipModel = mapper.Map<InternshipModel>(updateInternshipDTO);

        var updatedInternshipModel = await service.UpdateAsync(id, internshipModel, cancellationToken);

        var updatedInternshipDTO = mapper.Map<InternshipDTO>(updatedInternshipModel);

        return updatedInternshipDTO;
    }
}
