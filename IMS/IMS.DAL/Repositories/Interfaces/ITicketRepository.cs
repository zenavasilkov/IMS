using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.DAL.Repositories.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<List<Ticket>> GetTicketsByBoardId(Guid boardId); 

    Task<List<Ticket>> GetTicketsByBoardIdAndStatus(Guid boardId, TicketStatus status);
}
