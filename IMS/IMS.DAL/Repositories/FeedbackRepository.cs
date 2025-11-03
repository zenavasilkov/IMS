using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL.Repositories;

public class FeedbackRepository(ImsDbContext context, IFeedbackFilterBuilder filterBuilder) : Repository<Feedback>(context), IFeedbackRepository
{
    private readonly DbSet<Feedback> _feedbacks = context.Set<Feedback>();
    private readonly ImsDbContext _context = context;

    public async Task<List<Feedback>> GetFeedbacksAddressedToUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = _feedbacks
            .AsNoTracking()
            .Include(f => f.Ticket)
            .AsQueryable();

        var feedbacks = await filterBuilder
            .WithSentTo(userId)
            .Build(query)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);
            
        return feedbacks;
    }

    public async Task<List<Feedback>> GetFeedbacksByTicketIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = _feedbacks
            .AsNoTracking()
            .Include(f => f.Ticket)
            .AsQueryable();

        var feedbacks = await filterBuilder
            .WithTicket(id)
            .Build(query)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }

    public async Task<List<Feedback>> GetFeedbacksSentByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = _feedbacks
             .AsNoTracking()
             .Include(f => f.Ticket)
             .AsQueryable();

        var feedbacks = await filterBuilder
            .WithSentTo(userId)
            .Build(query)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);
            
        return feedbacks;
    }

    public override async Task<Feedback> CreateAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        var createdFeedback = await _feedbacks.AddAsync(feedback, cancellationToken); 
        await _context.SaveChangesAsync(cancellationToken);

        var createdFeedbackWithIncludes = await _feedbacks
            .Include(f => f.SentBy)
            .Include(f => f.AddressedTo)
            .Include(f => f.Ticket)
            .FirstAsync(f => f.Id == createdFeedback.Entity.Id, cancellationToken);

        return createdFeedbackWithIncludes;
    }
}
