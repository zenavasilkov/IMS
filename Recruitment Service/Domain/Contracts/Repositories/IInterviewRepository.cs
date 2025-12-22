using Domain.Entities;
using Pagination;

namespace Domain.Contracts.Repositories;

public interface IInterviewRepository : IGenericRepository<Interview>
{
    Task<PagedList<Interview>> GetInterviewsByCandidateId(Guid id, PaginationParameters paginationParameters,
        bool trackChanges = false, CancellationToken cancellationToken = default);
}
