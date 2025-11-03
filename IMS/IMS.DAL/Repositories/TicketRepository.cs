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
        
        public override async Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            var createdTicket = await _tickets.AddAsync(ticket, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var createdTicketWithIncludes = await _tickets
                .Include(t => t.Board)
                    .ThenInclude(b => b.CreatedTo)
                .FirstAsync(t => t.Id == createdTicket.Entity.Id, cancellationToken);

            return createdTicketWithIncludes;
        }

        public override async Task<Ticket> UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            var updatedTicket = _tickets.Update(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            var updatedTicketWithIncludes = await _tickets
                .Include(t => t.Board)
                    .ThenInclude(b => b.CreatedBy)
                .FirstAsync(t => t.Id == updatedTicket.Entity.Id, cancellationToken);

            return updatedTicketWithIncludes;
        }
    }
}
