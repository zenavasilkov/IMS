using System.Linq.Expressions;
using IMS.DAL.Entities;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool trachChanges = false,
        CancellationToken cancellationTokent = default);

    Task<PagedList<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? predicate,
        PaginationParameters paginationParameters, bool trachChanges = false, CancellationToken cancellationTokent = default);

    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
