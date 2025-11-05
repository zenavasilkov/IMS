using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.DAL.Repositories.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<List<Ticket>> GetTicketsByBoardId(Guid Id, CancellationToken cancellationToken); 

    Task<List<Ticket>> GetTicketsByBoardIdAndStatus(Guid Id, TicketStatus status, CancellationToken cancellationToken);
}
