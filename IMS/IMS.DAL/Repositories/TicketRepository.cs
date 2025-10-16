using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums; 

namespace IMS.DAL.Repositories
{
    public class TicketRepository(IMSDbContext context) : Repository<Ticket>(context), ITicketRepository
    {
        private readonly DbSet<Ticket> _tickets = context.Set<Ticket>();
        private readonly IMSDbContext _context = context;
        public async Task<List<Ticket>> GetTicketsByBoardId(Guid boardId, CancellationToken cancellationToken)
        {
            var tickets = await _tickets
                .AsNoTracking()
                .Where(t => t.BoardId == boardId)
                .ToListAsync(cancellationToken);

            return tickets;
        }

        public Task<List<Ticket>> GetTicketsByBoardIdAndStatus(Guid boardId, TicketStatus status, CancellationToken cancellationToken)
        {
           var tickets = _tickets
                .AsNoTracking()
                .Where(t => t.BoardId == boardId && t.Status == status)
                .ToListAsync(cancellationToken);

            return tickets;
        }
    }
}
