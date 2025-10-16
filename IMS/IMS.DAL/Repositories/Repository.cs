using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using System.Linq.Expressions; 

namespace IMS.DAL.Repositories;

public class Repository<TEntity>(IMSDbContext context) : IRepository<TEntity> where TEntity : EntityBase
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _dbSet.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken); 
        return createdEntity.Entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity); 
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, 
        bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        var entities =  await query.ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<PagedList<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? predicate, 
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default )
    {
        var query = _dbSet.AsQueryable();

        if(predicate is not null)
        {
            query = query.Where(predicate);
        } 

        query = trackChanges ? query : query.AsNoTracking();

        var list =  await query
                .OrderBy(e => e.Id)
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync(cancellationToken); 

        var totalCount = await query.CountAsync(cancellationToken);

        var PagedList = new PagedList<TEntity>(list, paginationParameters.PageNumber, paginationParameters.PageSize, totalCount);

        return PagedList;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();

        var entity = await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = _dbSet.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return existingEntity.Entity;
    }
}
