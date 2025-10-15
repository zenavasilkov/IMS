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
[Route(ApiRoutes.Tickets.Base)]
public class TicketsController(ITicketService ticketService, IService<FeedbackModel, Feedback> feedbackService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TicketDTO>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await ticketService.GetAllAsync(null, false, cancellationToken);

        var ticketDTOs = mapper.Map<IEnumerable<TicketDTO>>(tickets);

        return ticketDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<TicketDTO> GetById(Guid id, CancellationToken cancellationToken)
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
    public async Task<TicketDTO> Update(Guid id, [FromBody] UpdateTicketDTO updateTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(updateTicketDTO);

        var updatedTicketModel = await ticketService.UpdateAsync(id, ticketModel, cancellationToken); 

        var updatedTicketDTO = mapper.Map<TicketDTO>(updatedTicketModel);

        return updatedTicketDTO;
    }

    [HttpPatch(ApiRoutes.Tickets.AddFeedback)]
    public async Task<TicketDTO> AddFeedbackToTicket(Guid ticketId, Guid feedbackId, CancellationToken cancellationToken)
    { 
        var updatedTicketModel = await ticketService.AddFeedbackToTicket(ticketId, feedbackId, feedbackService, cancellationToken);

        var updatedTicketDTO = mapper.Map<TicketDTO>(updatedTicketModel);

        return updatedTicketDTO;
    }
}
