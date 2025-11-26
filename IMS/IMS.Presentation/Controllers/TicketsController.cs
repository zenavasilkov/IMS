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
[Route(ApiRoutes.Tickets.Base)]
public class TicketsController(ITicketService service, IMapper mapper) : ControllerBase
{
    [Authorize(Tickets.Read)]
    [HttpGet]
    public async Task<IEnumerable<TicketDto>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await service.GetAllAsync(cancellationToken: cancellationToken);

        var ticketDtos = mapper.Map<IEnumerable<TicketDto>>(tickets);

        return ticketDtos;
    }

    [Authorize(Tickets.Read)]
    [HttpGet(ApiRoutes.Id)]
    public async Task<TicketDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var ticket = await service.GetByIdAsync(id, cancellationToken); 

        var ticketDto = mapper.Map<TicketDto>(ticket);

        return ticketDto;
    }

    [Authorize(Tickets.Create)]
    [HttpPost]
    public async Task<TicketDto> Create([FromBody] CreateTicketDto createTicketDto, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(createTicketDto);

        var createdTicketModel = await service.CreateAsync(ticketModel, cancellationToken);

        var ticketDto = mapper.Map<TicketDto>(createdTicketModel);

        return ticketDto;
    }

    [Authorize(Tickets.Update)]
    [HttpPut(ApiRoutes.Id)]
    public async Task<TicketDto> Update([FromRoute] Guid id, [FromBody] UpdateTicketDto updateTicketDto, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(updateTicketDto);

        var updatedTicketModel = await service.UpdateAsync(id, ticketModel, cancellationToken); 

        var updatedTicketDto = mapper.Map<TicketDto>(updatedTicketModel);

        return updatedTicketDto;
    }

    [Authorize(Tickets.Read)]
    [HttpGet(ApiRoutes.Tickets.TicketsByBoardId)]
    public async Task<List<TicketDto>> GetTicketsByBoardId([FromRoute] Guid boardId, CancellationToken cancellationToken)
    {
        var tickets = await service.GetTicketsByBoardIdAsync(boardId, cancellationToken : cancellationToken);

        var ticketsDto = mapper.Map<List<TicketDto>>(tickets);

        return ticketsDto;
    }
}
