using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.BLL.Services.Interfaces;

public interface IFeedbackService : IService<FeedbackModel, Feedback>
{
    Task<PagedList<FeedbackModel>> GetAllAsync(
        PaginationParameters paginationParameters,
        FeedbackFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken);
}
