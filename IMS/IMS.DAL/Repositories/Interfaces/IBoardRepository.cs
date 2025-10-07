using IMS.DAL.Entities;

namespace IMS.DAL.Repositories.Interfaces;

public interface IBoardRepository : IRepository<Board>
{
    Task<List<Board>> GetBoardsCreatedByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<List<Board>> GetBoardsAssignedToUserAsync(Guid userId, CancellationToken cancellationToken = default); 
}
