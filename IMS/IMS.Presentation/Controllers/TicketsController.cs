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
[Route(ApiRoutes.Tickets.Base)]
public class TicketsController(ITicketService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TicketDto>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await service.GetAllAsync(cancellationToken: cancellationToken);

        var ticketDTOs = mapper.Map<IEnumerable<TicketDto>>(tickets);

        return ticketDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<TicketDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var ticket = await service.GetByIdAsync(id, cancellationToken); 

        var ticketDTO = mapper.Map<TicketDto>(ticket);

        return ticketDTO;
    }

    [HttpPost]
    public async Task<TicketDto> Create([FromBody] CreateTicketDto createTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(createTicketDTO);

        var createdTicketModel = await service.CreateAsync(ticketModel, cancellationToken);

        var ticketDTO = mapper.Map<TicketDto>(createdTicketModel);

        return ticketDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<TicketDto> Update([FromRoute] Guid id, [FromBody] UpdateTicketDto updateTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(updateTicketDTO);

        var updatedTicketModel = await service.UpdateAsync(id, ticketModel, cancellationToken); 

        var updatedTicketDTO = mapper.Map<TicketDto>(updatedTicketModel);

        return updatedTicketDTO;
    }

    [HttpGet(ApiRoutes.Tickets.TicketsByBoardId)]
    public async Task<List<TicketDto>> GetTicketsByBoardId([FromRoute] Guid boardId, CancellationToken cancellationToken)
    {
        var tickets = await service.GetTicketsByBoardIdAsync(boardId, cancellationToken : cancellationToken);

        var ticketsDTO = mapper.Map<List<TicketDto>>(tickets);

        return ticketsDTO;
    }
}
