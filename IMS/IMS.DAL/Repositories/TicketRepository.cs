using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums; 

namespace IMS.DAL.Repositories
{
    public class TicketRepository(ImsDbContext context, ITicketFilterBuilder filterBuilder) : Repository<Ticket>(context), ITicketRepository
    {
        private readonly DbSet<Ticket> _tickets = context.Set<Ticket>();

        public async Task<List<Ticket>> GetTicketsByBoardId(Guid boardId, CancellationToken cancellationToken)
        {
            var query = _tickets
                 .AsNoTracking()
                 .Include(t => t.Board)
                 .AsQueryable();

            var tickets = await filterBuilder
                .WithBoard(boardId)
                .Build(query)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken);

            return tickets;
        }

        public async Task<List<Ticket>> GetTicketsByBoardIdAndStatus(Guid boardId, TicketStatus status, CancellationToken cancellationToken)
        {
            var query = _tickets
                 .AsNoTracking()
                 .Include(t => t.Board)
                 .AsQueryable();

             var tickets = await filterBuilder
                 .WithBoard(boardId)
                 .WithStatus(status)
                 .Build(query)
                 .OrderBy(t => t.Id)
                 .ToListAsync(cancellationToken);

            return tickets;
        }
    }
}
