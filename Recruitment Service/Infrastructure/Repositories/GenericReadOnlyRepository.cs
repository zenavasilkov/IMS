using Domain.Contracts.Repositories;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class GenericReadOnlyRepository<TEntity>(RecruitmentDbContext context) : IGenericReadOnlyRepository<TEntity> where TEntity : Entity
{
    protected readonly RecruitmentDbContext _context = context;

    public async Task<PagedList<TEntity>> GetByConditionAsync(
            Expression<Func<TEntity, bool>> expression,
            PaginationParameters paginationParameters, 
            bool trackChanges = false,
            CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>()
            .Where(expression);

        var totalCount = await query.CountAsync(cancellationToken);

        query = trackChanges ? query : query.AsNoTracking();

        var list = await query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(cancellationToken);

        var pagedList = new PagedList<TEntity>(list, paginationParameters.PageNumber, paginationParameters.PageSize, totalCount);

        return pagedList;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().Where(e => e.Id == id);

        query = trackChanges ? query : query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}
