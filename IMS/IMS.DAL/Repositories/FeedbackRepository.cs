using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using System.Linq.Expressions;
using Shared.Filters;

namespace IMS.DAL.Repositories;

public class FeedbackRepository(ImsDbContext context, IRepository<Feedback> repository) : IFeedbackRepository
{
    private readonly DbSet<Feedback> _feedbacks = context.Set<Feedback>();

   public async Task<Feedback> CreateAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        var createdFeedback = await _feedbacks.AddAsync(feedback, cancellationToken); 
        await context.SaveChangesAsync(cancellationToken);

        var createdFeedbackWithIncludes = await _feedbacks
            .Include(f => f.SentBy)
            .Include(f => f.AddressedTo)
            .Include(f => f.Ticket)
            .FirstAsync(f => f.Id == createdFeedback.Entity.Id, cancellationToken);

        return createdFeedbackWithIncludes;
    }

    public Task<PagedList<Feedback>> GetPagedAsync(Expression<Func<Feedback, bool>>? predicate,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<Feedback?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public Task<Feedback> UpdateAsync(Feedback entity, CancellationToken cancellationToken = default) =>
        repository.UpdateAsync(entity, cancellationToken);

    public Task DeleteAsync(Feedback entity, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(entity, cancellationToken);
    
    public Task<List<Feedback>> GetAllAsync(Expression<Func<Feedback, bool>>? predicate = null,
        bool trackChanges = false, CancellationToken cancellationTokent = default) =>
        repository.GetAllAsync(predicate, trackChanges, cancellationTokent);
    
    public async Task<PagedList<Feedback>> GetAllAsync(
        PaginationParameters paginationParameters,
        FeedbackFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        var query = _feedbacks.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();
        query = ApplyFilters(query, filter);

        var feedbacks = await query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        var feedbackModels = new PagedList<Feedback>(feedbacks, paginationParameters.PageNumber, 
            paginationParameters.PageSize, totalCount);

        return feedbackModels;
    }

    private static IQueryable<Feedback> ApplyFilters(IQueryable<Feedback> query, FeedbackFilteringParameters filter)
    {
        query = new FeedbackFilterBuilder()
            .WithComment(filter.Comment)
            .SentBy(filter.SentById)
            .SentTo(filter.SentToId)
            .WithTicket(filter.TicketId)
            .Build(query);

        return query;
    }
}
