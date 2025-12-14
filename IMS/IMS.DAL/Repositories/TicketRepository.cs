using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Pagination;
using System.Linq.Expressions;
using Shared.Filters;

namespace IMS.DAL.Repositories;

public class TicketRepository(ImsDbContext context, IRepository<Ticket> repository) : ITicketRepository
{
    private readonly DbSet<Ticket> _tickets = context.Set<Ticket>();
   
    public async Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        var createdTicket = await _tickets.AddAsync(ticket, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var createdTicketWithIncludes = await _tickets
            .Include(t => t.Board)
                .ThenInclude(b => b.CreatedTo)
            .FirstAsync(t => t.Id == createdTicket.Entity.Id, cancellationToken);

        return createdTicketWithIncludes;
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        var updatedTicket = _tickets.Update(ticket);
        await context.SaveChangesAsync(cancellationToken);

        var updatedTicketWithIncludes = await _tickets
            .Include(t => t.Board)
                .ThenInclude(b => b.CreatedBy)
            .FirstAsync(t => t.Id == updatedTicket.Entity.Id, cancellationToken);

        return updatedTicketWithIncludes;
    }

    public Task<List<Ticket>> GetAllAsync(Expression<Func<Ticket, bool>>? predicate = null, 
        bool trackChanges = false, CancellationToken cancellationTokent = default) =>
        repository.GetAllAsync(predicate, trackChanges, cancellationTokent);

    public Task<PagedList<Ticket>> GetPagedAsync(Expression<Func<Ticket, bool>>? predicate, 
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<Ticket?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public Task DeleteAsync(Ticket entity, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(entity, cancellationToken);

    public async Task<PagedList<Ticket>> GetAllAsync(
        PaginationParameters paginationParameters,
        TicketFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var query = _tickets.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();
        query = ApplyFilters(query, filter);

        var tickets = await query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        var ticketModals = new PagedList<Ticket>(tickets, paginationParameters.PageNumber,
            paginationParameters.PageSize, totalCount);

        return ticketModals;
    }
    
    private static IQueryable<Ticket> ApplyFilters(IQueryable<Ticket> query, TicketFilteringParameters filter)
    {
        query = new TicketFilterBuilder()
            .WithTitle(filter.Title)
            .WithDescription(filter.Description)
            .WithStatus(filter.Status)
            .WithBoard(filter.BoardId)
            .Build(query);
        
        return query;
    }
}
