using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL.Repositories;

public class FeedbackRepository(IMSDbContext context, IFeedbackFilterBuilder filterBuilder) : Repository<Feedback>(context), IFeedbackRepository
{
    private readonly DbSet<Feedback> _feedbacks = context.Set<Feedback>();

    public async Task<List<Feedback>> GetFeedbacksAddressedToUserAsync(Guid sentToId, CancellationToken cancellationToken = default)
    {
        var query = _feedbacks
            .AsNoTracking()
            .Include(f => f.Ticket)
            .AsQueryable();

        var feedbacks = await filterBuilder
            .WithSentTo(sentToId)
            .Build(query)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);
            
        return feedbacks;
    }

    public async Task<List<Feedback>> GetFeedbacksForTicketAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var query = _feedbacks
            .AsNoTracking()
            .Include(f => f.Ticket)
            .AsQueryable();

        var feedbacks = await filterBuilder
            .WithTicket(ticketId)
            .Build(query)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }

    public async Task<List<Feedback>> GetFeedbacksSentByUserAsync(Guid sentById, CancellationToken cancellationToken = default)
    {
        var query = _feedbacks
             .AsNoTracking()
             .Include(f => f.Ticket)
             .AsQueryable();

        var feedbacks = await filterBuilder
            .WithSentTo(sentById)
            .Build(query)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);
            
        return feedbacks;
    }
}
