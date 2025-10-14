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
public class TicketsController(ITicketService ticketService, IFeedbackService feedbackService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await ticketService.GetAllAsync(null, false, cancellationToken);
         
        if (tickets.Count == 0) return NoContent();

        var ticketDTOs = mapper.Map<IEnumerable<TicketDTO>>(tickets);

        return Ok(ticketDTOs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketDTO>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var ticket = await ticketService.GetByIdAsync(id, cancellationToken);

        if (ticket is null) return NotFound(new { message = $"Ticket with ID {id} was not found." }); 

        var ticketDTO = mapper.Map<TicketDTO>(ticket);

        return Ok(ticketDTO);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDTO>> Create([FromBody] CreateTicketDTO createTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(createTicketDTO);

        var createdTicketModel = await ticketService.CreateAsync(ticketModel, cancellationToken);

        var ticketDTO = mapper.Map<TicketDTO>(createdTicketModel);

        return CreatedAtAction(nameof(GetById), new { id = ticketDTO.Id}, ticketDTO);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TicketDTO>> Update(Guid id, [FromBody] UpdateTicketDTO updateTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = mapper.Map<TicketModel>(updateTicketDTO); 

        ticketModel.Id = id; 

        var updatedTicketModel = await ticketService.UpdateAsync(ticketModel, cancellationToken);

        if (updatedTicketModel is null) return NotFound(new { message = $"Ticket with ID {id} was not found." }); 

        var updatedTicketDTO = mapper.Map<TicketDTO>(updatedTicketModel);

        return updatedTicketDTO;
    }

    [HttpPatch("{ticketId:guid}/feedbacks/{feedbackId:guid}")]
    public async Task<ActionResult<TicketDTO>> AddFeedbackToTicketById(Guid ticketId, Guid feedbackId, CancellationToken cancellationToken)
    {
        var ticketModel = await ticketService.GetByIdAsync(ticketId, cancellationToken);

        if (ticketModel is null)
        {
            return NotFound(new { message = $"Ticket with ID {ticketId} was not found" });
        }

        var feedbackModel = await feedbackService.GetByIdAsync(feedbackId, cancellationToken);

        if (feedbackModel is null)
        {
            return NotFound(new { message = $"Feedback with ID {feedbackId} was not found" });
        }

        ticketModel.Feedbacks ??= [];

        ticketModel.Feedbacks.Add(feedbackModel);

        var updatedTicketModel = await ticketService.UpdateAsync(ticketModel, cancellationToken);

        var updatedTicketDTO = mapper.Map<TicketDTO>(updatedTicketModel);

        return Ok(updatedTicketDTO);
    }
}
