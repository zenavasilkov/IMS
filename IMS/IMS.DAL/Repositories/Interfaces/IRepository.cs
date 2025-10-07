using System.Linq.Expressions;
using IMS.DAL.Entities;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationTokent = default);

    Task<List<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? predicate, 
        PaginationParameters paginationParameters, CancellationToken cancellationTokent = default);

    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
