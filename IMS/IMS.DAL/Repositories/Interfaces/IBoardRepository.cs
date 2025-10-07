using IMS.DAL.Entities;

namespace IMS.DAL.Repositories.Interfaces;

public interface IBoardRepository : IRepository<Board>
{
    Task<List<Board>> GetBoardsCreatedByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Board> GetBoardAssignedToUserAsync(Guid userId, CancellationToken cancellationToken = default); 

    Task<Board> GetBoardByTicketAsync(Guid ticketId, CancellationToken cancellationToken = default); 
}
