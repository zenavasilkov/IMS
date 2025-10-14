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
    private readonly ITicketService _ticketService = ticketService;
    private readonly IFeedbackService _feedbackService = feedbackService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await _ticketService.GetAllAsync(null, false, cancellationToken);

        var ticketDTOs = _mapper.Map<IEnumerable<TicketDTO>>(tickets);

        if (!ticketDTOs.Any()) return NoContent();

        return Ok(ticketDTOs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketDTO>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.GetByIdAsync(id, cancellationToken);

        if (ticket is null) return NotFound(new { message = $"Ticket with ID {id} was not found." }); 

        var ticketDTO = _mapper.Map<TicketDTO>(ticket);

        return Ok(ticketDTO);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDTO>> Create([FromBody] CreateTicketDTO createTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = _mapper.Map<TicketModel>(createTicketDTO);

        var createdTicketModel = await _ticketService.CreateAsync(ticketModel, cancellationToken);

        var ticketDTO = _mapper.Map<TicketDTO>(createdTicketModel);

        return CreatedAtAction(nameof(GetById), new { id = ticketDTO.Id}, ticketDTO);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TicketDTO>> Update(Guid id, [FromBody] UpdateTicketDTO updateTicketDTO, CancellationToken cancellationToken)
    {
        var ticketModel = _mapper.Map<TicketModel>(updateTicketDTO); 

        ticketModel.Id = id; 

        var updatedTicketModel = await _ticketService.UpdateAsync(ticketModel, cancellationToken);

        if (updatedTicketModel is null) return NotFound(new { message = $"Ticket with ID {id} was not found." }); 

        var updatedTicketDTO = _mapper.Map<TicketDTO>(updatedTicketModel);

        return updatedTicketDTO;
    }

    [HttpPatch("{ticketId:guid}/feedbacks/{feedbackId:guid}")]
    public async Task<ActionResult<TicketDTO>> AddFeedbackToTicketById(Guid ticketId, Guid feedbackId, CancellationToken cancellationToken)
    {
        var ticketModel = await _ticketService.GetByIdAsync(ticketId, cancellationToken);

        if (ticketModel is null)
        {
            return NotFound(new { message = $"Ticket with ID {ticketId} was not found" });
        }

        var feedbackModel = await _feedbackService.GetByIdAsync(feedbackId, cancellationToken);

        if (feedbackModel is null)
        {
            return NotFound(new { message = $"Feedback with ID {feedbackId} was not found" });
        }

        ticketModel.Feedbacks ??= [];

        ticketModel.Feedbacks.Add(feedbackModel);

        var updatedTicketModel = await _ticketService.UpdateAsync(ticketModel, cancellationToken);

        var updatedTicketDTO = _mapper.Map<TicketDTO>(updatedTicketModel);

        return Ok(updatedTicketDTO);
    }
}
