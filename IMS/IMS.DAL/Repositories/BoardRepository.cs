using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL.Repositories;

public class BoardRepository(IMSDbContext context) : Repository<Board>(context), IBoardRepository
{
    private readonly DbSet<Board> _boards = context.Set<Board>();
    private readonly IMSDbContext _context = context;

    public override async Task<Board> UpdateAsync(Board board, CancellationToken cancellationToken = default)
    {
        var existingBoard = await _boards.FirstAsync(i => i.Id == board.Id, cancellationToken);

        existingBoard.Title = board.Title;

        existingBoard.Description = board.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return existingBoard;
    }

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
