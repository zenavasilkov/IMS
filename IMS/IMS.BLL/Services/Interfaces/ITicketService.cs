using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.BLL.Services.Interfaces;

public interface ITicketService : IService<TicketModel, Ticket>
{
    Task<PagedList<TicketModel>> GetAllAsync(
        PaginationParameters paginationParameters,
        TicketFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken = default);
}
