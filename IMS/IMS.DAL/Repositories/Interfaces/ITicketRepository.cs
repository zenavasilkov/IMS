using IMS.DAL.Entities;
using IMS.DAL.Enums;

namespace IMS.DAL.Repositories.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<List<Ticket>> GetTicketsByBoard(Board board); 

    Task<List<Ticket>> GetTicketsByBoardAndStatus(Board board, TicketStatus status);
}
