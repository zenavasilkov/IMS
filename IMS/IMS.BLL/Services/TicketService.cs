using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class TicketService(ITicketRepository repository, IMapper mapper) : Service<TicketModel, Ticket>(repository, mapper), ITicketService
{
    public async Task<TicketModel?> AddFeedbackToTicket(Guid ticketId, Guid feedbackId, 
        IService<FeedbackModel, Feedback> feedbackService, CancellationToken cancellationToken = default)
    {
        var ticket = await GetByIdAsync(ticketId, cancellationToken) 
            ?? throw new Exception($"Ticket with ID {ticketId} was not found"); // TODO: Add custom exception

        var feedback = await feedbackService.GetByIdAsync(feedbackId, cancellationToken)
            ?? throw new Exception($"Feedback with ID {feedbackId} was not found"); // TODO: Add custom exception

        ticket.Feedbacks ??= [];

        ticket.Feedbacks.Add(feedback);

        var updatedTicket = await UpdateAsync(ticketId, ticket, cancellationToken);

        return updatedTicket;
    }
}
