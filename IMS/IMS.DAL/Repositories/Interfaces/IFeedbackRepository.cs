using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces;

public interface IFeedbackRepository : IRepository<Feedback>
{
    Task<PagedList<Feedback>> GetAllAsync(
        PaginationParameters paginationParameters,
        FeedbackFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken);
}
