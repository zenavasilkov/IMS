using Domain.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class EmployeeRepository(IGenericRepository<Employee> repository, RecruitmentDbContext context) : IEmployeeRepository
{
    public Task<Employee> CreateAsync(Employee entity, CancellationToken cancellationToken = default) => repository.CreateAsync(entity, cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => repository.DeleteAsync(id, cancellationToken);

    public Task<PagedList<Employee>> GetByConditionAsync(Expression<Func<Employee, bool>> expression,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetByConditionAsync(expression, paginationParameters, trackChanges, cancellationToken);

    public async Task<Employee?> GetByEmailAsync(string email, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Employee>().AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();

        var employee = await query.FirstOrDefaultAsync(e => e.Email == email, cancellationToken);

        return employee;
    }

    public Task<Employee?> GetByIdAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public Task<Employee> UpdateAsync(Employee entity, CancellationToken cancellationToken = default) =>
        repository.UpdateAsync(entity, cancellationToken);
}
