using IMS.DAL.Entities;

namespace IMS.DAL.Repositories.Interfaces;

public interface IBoardRepository : IRepository<Board>
{
    Task<List<Board>> GetBoardsCreatedByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Board?> GetBoardAssignedToUserAsync(Guid userId, CancellationToken cancellationToken = default); 

    Task<Board> GetBoardByTicketIdAsync(Guid id, CancellationToken cancellationToken = default); 
}
