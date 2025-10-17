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
public class TicketsController(ITicketService ticketService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TicketDTO>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await ticketService.GetAllAsync(cancellationToken: cancellationToken);

        var ticketDTOs = mapper.Map<IEnumerable<TicketDTO>>(tickets);

        return ticketDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<TicketDTO> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var ticket = await ticketService.GetByIdAsync(id, cancellationToken); 

        var ticketDTO = mapper.Map<TicketDTO>(ticket);

        return ticketDTO;
    }

    [HttpPost]
    public async Task<TicketDTO> Create([FromBody] CreateTicketDTO createTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(createTicketDTO);

        var createdTicketModel = await ticketService.CreateAsync(ticketModel, cancellationToken);

        var ticketDTO = mapper.Map<TicketDTO>(createdTicketModel);

        return ticketDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<TicketDTO> Update([FromRoute] Guid id, [FromBody] UpdateTicketDTO updateTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(updateTicketDTO);

        var updatedTicketModel = await ticketService.UpdateAsync(id, ticketModel, cancellationToken); 

        var updatedTicketDTO = mapper.Map<TicketDTO>(updatedTicketModel);

        return updatedTicketDTO;
    }

    [HttpGet]
    public async Task<List<TicketDTO>> GetTicketsByBoardId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var tickets = await GetTicketsByBoardId(id, cancellationToken : cancellationToken);

        var ticketsDTO = mapper.Map<List<TicketDTO>>(tickets);

        return ticketsDTO; ;
    }
}
