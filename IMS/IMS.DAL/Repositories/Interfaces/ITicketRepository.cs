using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<PagedList<Ticket>> GetAllAsync(
        PaginationParameters paginationParameters,
        TicketFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken = default);
}
