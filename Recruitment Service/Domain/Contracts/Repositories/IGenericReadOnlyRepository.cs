using Domain.Primitives;
using Pagination;
using System.Linq.Expressions;

namespace Domain.Contracts.Repositories;

public interface IGenericReadOnlyRepository<T>
    where T : Entity
{
    Task<PagedList<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression,
        PaginationParameters paginationParameters,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default);
}
