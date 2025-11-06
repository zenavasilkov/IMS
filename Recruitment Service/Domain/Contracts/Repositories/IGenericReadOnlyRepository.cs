using Domain.Entities;
using Pagination;
using System.Linq.Expressions;

namespace Domain.Contracts.Repositories;

public interface IGenericReadOnlyRepository<T>
    where T : Entity
{
    Task<PagedList<T>> FindByConditionAsync(
        Expression<Func<T, bool>> expression,
        PaginationParameters paginationParameters,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<T?> FindByIdAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default);
}
