using IMS.DAL.Entities; 

namespace IMS.DAL.Repositories.Interfaces;

public interface IFeedbackRepository : IRepository<Feedback>
{
    Task<List<Feedback>> GetFeedbacksForTicketAsync(Ticket ticket, CancellationToken cancellationToken = default);

    Task<List<Feedback>> GetFeedbacksSentByUserAsync(User user, CancellationToken cancellationToken = default);

    Task<List<Feedback>> GetFeedbacksAddressedToUserAsync(User user, CancellationToken cancellationToken = default);
}
