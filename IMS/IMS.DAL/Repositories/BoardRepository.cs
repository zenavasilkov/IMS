using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using System.Linq.Expressions;
using IMS.DAL.Builders;
using Shared.Filters;

namespace IMS.DAL.Repositories;

public class BoardRepository(ImsDbContext context, IRepository<Board> repository) : IBoardRepository
{
    private readonly DbSet<Board> _boards = context.Set<Board>();

    public Task<Board> CreateAsync(Board entity, CancellationToken cancellationToken = default) => repository.CreateAsync(entity, cancellationToken);

    public Task DeleteAsync(Board entity, CancellationToken cancellationToken = default) => repository.DeleteAsync(entity, cancellationToken);

    public Task<List<Board>> GetAllAsync(Expression<Func<Board, bool>>? predicate = null, bool trackChanges = false, CancellationToken cancellationTokent = default) =>
        repository.GetAllAsync(predicate, trackChanges, cancellationTokent);
    
    public async Task<PagedList<Board>> GetAllAsync(
        PaginationParameters paginationParameters,
        BoardFilteringParameters filter,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        var query = _boards.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();
        query = ApplyFilters(query, filter);

        var boards = await query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedList = new PagedList<Board>(boards, paginationParameters.PageNumber, 
            paginationParameters.PageSize, totalCount);

        return pagedList;
    }

    private static IQueryable<Board> ApplyFilters(IQueryable<Board> query, BoardFilteringParameters filter)
    {
        query = new BoardFilterBuilder()
            .WithTitle(filter.Title)
            .WithDescription(filter.Description)
            .CreatedBy(filter.CreatedById)
            .CreatedTo(filter.CreatedToId)
            .Build(query);

        return query;
    }

    public async Task<Board?> GetBoardByTicketIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var board = await _boards
            .Include(b => b.Tickets)
            .FirstAsync(b => b.Tickets.Any(t => t.Id == id), cancellationToken);

        return board!;
    }

    public Task<Board?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public Task<PagedList<Board>> GetPagedAsync(Expression<Func<Board, bool>>? predicate,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<Board> UpdateAsync(Board entity, CancellationToken cancellationToken = default) => repository.UpdateAsync(entity, cancellationToken);
}
