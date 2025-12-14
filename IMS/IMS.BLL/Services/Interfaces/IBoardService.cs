using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.BLL.Services.Interfaces;

public interface IBoardService : IService<BoardModel, Board>
{
    Task<PagedList<BoardModel>> GetAllAsync(
        PaginationParameters paginationParameters,
        BoardFilteringParameters filteringParameters,
        bool trackChanges,
        CancellationToken cancellationToken = default
    );
}
