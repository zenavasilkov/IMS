using IMS.BLL.Models;
using IMS.DAL.Entities;

namespace IMS.BLL.Services.Interfaces;

public interface IBoardService : IService<BoardModel, Board>
{
    Task<BoardModel?> AddTicketToBoard(Guid boardId, Guid ticketId, IService<TicketModel, Ticket> ticketService, CancellationToken cancellationToken = default);
}
