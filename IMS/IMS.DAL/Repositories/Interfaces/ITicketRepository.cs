using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.DAL.Repositories.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<List<Ticket>> GetTicketsByBoardId(Guid boardId, CancellationToken cancellationToken); 

    Task<List<Ticket>> GetTicketsByBoardIdAndStatus(Guid boardId, TicketStatus status, CancellationToken cancellationToken);
}
