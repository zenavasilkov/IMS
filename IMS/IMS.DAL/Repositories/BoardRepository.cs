using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using System.Linq.Expressions;

namespace IMS.DAL.Repositories;

public class BoardRepository(ImsDbContext context, IRepository<Board> repository) : IBoardRepository
{
    private readonly DbSet<Board> _boards = context.Set<Board>();

    public Task<Board> CreateAsync(Board entity, CancellationToken cancellationToken = default) => repository.CreateAsync(entity, cancellationToken);

    public Task DeleteAsync(Board entity, CancellationToken cancellationToken = default) => repository.DeleteAsync(entity, cancellationToken);

    public Task<List<Board>> GetAllAsync(Expression<Func<Board, bool>>? predicate = null, bool trackChanges = false, CancellationToken cancellationTokent = default) =>
        repository.GetAllAsync(predicate, trackChanges, cancellationTokent);

    public async Task<Board?> GetBoardAssignedToUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var board = await _boards
            .FirstOrDefaultAsync(b => b.CreatedToId == userId, cancellationToken);

        return board;
    }

    public async Task<Board> GetBoardByTicketIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var board = await _boards
            .Include(b => b.Tickets)
            .FirstAsync(b => b.Tickets.Any(t => t.Id == id), cancellationToken);

        return board!;
    }

    public async Task<List<Board>> GetBoardsCreatedByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var boards = await _boards
            .AsNoTracking()
            .Where(b => b.CreatedById == userId)
            .OrderBy(b => b.Id)
            .ToListAsync(cancellationToken);

        return boards;
    }

    public Task<Board?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public Task<PagedList<Board>> GetPagedAsync(Expression<Func<Board, bool>>? predicate,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<Board> UpdateAsync(Board entity, CancellationToken cancellationToken = default) => repository.UpdateAsync(entity, cancellationToken);
}
