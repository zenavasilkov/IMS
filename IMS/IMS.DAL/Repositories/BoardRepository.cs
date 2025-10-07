using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL.Repositories;

public class BoardRepository(IMSDbContext context) : Repository<Board>(context), IBoardRepository
{
    private readonly DbSet<Board> _boards = context.Set<Board>();
    public async Task<Board?> GetBoardAssignedToUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var board = await _boards
            .FirstOrDefaultAsync(b => b.CreatedToId == userId, cancellationToken);

        return board;
    }

    public async Task<Board> GetBoardByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var board = await _boards
            .Include(b => b.Tickets)
            .FirstAsync(b => b.Tickets.Any(t => t.Id == ticketId), cancellationToken);

        return board!;
    }

    public async Task<List<Board>> GetBoardsCreatedByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var boards = await _boards
            .AsNoTracking()
            .Where(b => b.CreatedById == userId)
            .ToListAsync(cancellationToken);

        return boards;
    }
}
