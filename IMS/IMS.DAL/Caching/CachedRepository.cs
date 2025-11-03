using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Shared.Pagination;
using System.Linq.Expressions;

namespace IMS.DAL.Caching;

public class CachedRepository<TEntity>(IRepository<TEntity> decorated,
    IDistributedCache distributedCache, ImsDbContext context) : IRepository<TEntity> where TEntity : EntityBase
{
    private readonly ImsDbContext _context = context;

    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        => decorated.CreateAsync(entity, cancellationToken);

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        => decorated.DeleteAsync(entity, cancellationToken);

    public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null,
        bool trackChanges = false, CancellationToken cancellationTokent = default) =>
        decorated.GetAllAsync(predicate, trackChanges, cancellationTokent);

    public async Task<TEntity?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var chachedEntity = await distributedCache.GetStringAsync(id.ToString(), cancellationToken);

        if (string.IsNullOrEmpty(chachedEntity))
        {
            var entity = await decorated.GetByIdAsync(id, trackChanges, cancellationToken);

            if (entity is not null)
                await distributedCache.SetStringAsync(id.ToString(), JsonConvert.SerializeObject(entity), cancellationToken);

            return entity;
        }

        var entityFromCache = (TEntity?)JsonConvert.DeserializeObject(chachedEntity);

        if (entityFromCache is not null) _context.Set<TEntity>().Attach(entityFromCache);

        return entityFromCache;
    }

    public Task<PagedList<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? predicate,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        decorated.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        decorated.UpdateAsync(entity, cancellationToken);
}
