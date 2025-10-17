using IMS.BLL.Models;
using IMS.DAL.Entities;

namespace IMS.BLL.Services.Interfaces;

public interface ITicketService : IService<TicketModel, Ticket>
{
    Task<List<TicketModel>> GetTicketsByBoardIdAsync(Guid id, bool trackChanges = false, 
        CancellationToken cancellationToken = default);
}
