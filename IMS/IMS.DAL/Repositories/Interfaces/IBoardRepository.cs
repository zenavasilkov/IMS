using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces;

public interface IBoardRepository : IRepository<Board>
{
    Task<PagedList<Board>> GetAllAsync(
        PaginationParameters paginationParameters,
        BoardFilteringParameters filter,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<Board?> GetBoardByTicketIdAsync(Guid id, CancellationToken cancellationToken = default); 
}
