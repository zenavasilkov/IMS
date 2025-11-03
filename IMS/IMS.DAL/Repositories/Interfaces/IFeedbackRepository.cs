using IMS.DAL.Entities; 

namespace IMS.DAL.Repositories.Interfaces;

public interface IFeedbackRepository : IRepository<Feedback>
{
    Task<List<Feedback>> GetFeedbacksByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default);

    Task<List<Feedback>> GetFeedbacksSentByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<List<Feedback>> GetFeedbacksAddressedToUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
