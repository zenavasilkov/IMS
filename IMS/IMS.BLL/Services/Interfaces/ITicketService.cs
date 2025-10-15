using IMS.BLL.Models;
using IMS.DAL.Entities;

namespace IMS.BLL.Services.Interfaces;

public interface ITicketService : IService<TicketModel, Ticket>
{
    Task<TicketModel?> AddFeedbackToTicket(Guid ticketId, Guid feedbackId, IService<FeedbackModel, Feedback> feedbackService, CancellationToken cancellationToken = default);
}
