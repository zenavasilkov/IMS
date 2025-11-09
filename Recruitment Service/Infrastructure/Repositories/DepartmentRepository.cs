using Domain.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class DepartmentRepository(RecruitmentDbContext context, IGenericRepository<Department> repository) : IDepartmentRepository
{
    public Task<Department> CreateAsync(Department entity, CancellationToken cancellationToken = default) =>
        repository.CreateAsync(entity, cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(id, cancellationToken);

    public Task<PagedList<Department>> GetByConditionAsync(Expression<Func<Department, bool>> expression,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetByConditionAsync(expression, paginationParameters, trackChanges, cancellationToken);

    public Task<Department?> GetByIdAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public async Task<Department?> GetByNameAsync(string name, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        var query = context.Departments.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();
            
        var department = await query.FirstOrDefaultAsync(d => d.Name == name, cancellationToken);

        return department;
    }

    public Task<Department> UpdateAsync(Department entity, CancellationToken cancellationToken = default) =>
        repository.UpdateAsync(entity, cancellationToken);
}
