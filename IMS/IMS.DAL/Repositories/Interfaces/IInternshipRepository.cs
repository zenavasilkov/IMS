using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces;

public interface IInternshipRepository : IRepository<Internship>
{
    Task<PagedList<Internship>> GetAllAsync(
        PaginationParameters paginationParameters,
        InternshipFilteringParameters filter,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);
}
