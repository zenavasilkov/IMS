using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions; 

namespace IMS.DAL.Repositories;

public class Repository<TEntity>(IMSDbContext context) : IRepository<TEntity> where TEntity : EntityBase
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken); 
        return entity;
    }

    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, 
        CancellationToken cancellationTokent = default)
    {
        var query = _dbSet.AsQueryable();

        if(predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationTokent);
    }

    public async Task<List<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? predicate, 
        int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if(predicate is not null)
        {
            query = query.Where(predicate);
        } 

        return await query
                .OrderBy(e => e.Id) //To guarantee the same result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken); 
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = _dbSet.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return existingEntity.Entity;
    }
}
