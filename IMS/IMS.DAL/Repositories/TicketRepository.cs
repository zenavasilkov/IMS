using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Pagination;
using System.Linq.Expressions;

namespace IMS.DAL.Repositories;

public class TicketRepository(ImsDbContext context, ITicketFilterBuilder filterBuilder, IRepository<Ticket> repository) : ITicketRepository
{
    private readonly DbSet<Ticket> _tickets = context.Set<Ticket>();
    private readonly ImsDbContext _context = context;

    public async Task<List<Ticket>> GetTicketsByBoardId(Guid id, CancellationToken cancellationToken)
    {
        var query = _tickets
             .AsNoTracking()
             .Include(t => t.Board)
             .AsQueryable();

        var tickets = await filterBuilder
            .WithBoard(id)
            .Build(query)
            .OrderBy(t => t.Id)
            .ToListAsync(cancellationToken);

        return tickets;
    }

    public async Task<List<Ticket>> GetTicketsByBoardIdAndStatus(Guid id, TicketStatus status, CancellationToken cancellationToken)
    {
        var query = _tickets
             .AsNoTracking()
             .Include(t => t.Board)
             .AsQueryable();

        var tickets = await filterBuilder
             .WithBoard(id)
             .WithStatus(status)
             .Build(query)
             .OrderBy(t => t.Id)
             .ToListAsync(cancellationToken);

        return tickets;
    }
    
    public async Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        var createdTicket = await _tickets.AddAsync(ticket, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var createdTicketWithIncludes = await _tickets
            .Include(t => t.Board)
                .ThenInclude(b => b.CreatedTo)
            .FirstAsync(t => t.Id == createdTicket.Entity.Id, cancellationToken);

        return createdTicketWithIncludes;
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        var updatedTicket = _tickets.Update(ticket);
        await _context.SaveChangesAsync(cancellationToken);

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
}
