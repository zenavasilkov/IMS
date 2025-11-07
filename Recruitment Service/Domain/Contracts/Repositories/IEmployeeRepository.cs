using Domain.Entities;

namespace Domain.Contracts.Repositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee?> GetByEmailAsync(string email, bool trackChanges = true, CancellationToken cancellationToken = default);
}
