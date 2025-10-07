using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL.Repositories;

public class FeedbackRepository(IMSDbContext context) : Repository<Feedback>(context), IFeedbackRepository
{
    private readonly DbSet<Feedback> _feedbacks = context.Set<Feedback>();
    public async Task<List<Feedback>> GetFeedbacksAddressedToUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var feedbacks = await _feedbacks
            .AsNoTracking()
            .Where(f => f.AddressedToId == userId)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }

    public async Task<List<Feedback>> GetFeedbacksForTicketAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var feedbacks = await _feedbacks
            .AsNoTracking()
            .Where(f => f.TicketId == ticketId)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }

    public async Task<List<Feedback>> GetFeedbacksSentByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var feedbacks = await _feedbacks
            .AsNoTracking()
            .Where(f => f.SentById == userId)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }
}
