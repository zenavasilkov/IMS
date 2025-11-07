using Domain.Contracts.Repositories;
using Domain.Primitives;

namespace Infrastructure.Repositories;

public class GenericRepository<TEntity>(RecruitmentDbContext context) :
    GenericReadOnlyRepository<TEntity>(context),
    IGenericRepository<TEntity> where TEntity : Entity
{
    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);

        if (entity is null) return false;

        _context.Set<TEntity>().Remove(entity);

        return await _context.SaveChangesAsync(cancellationToken) == 1;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
