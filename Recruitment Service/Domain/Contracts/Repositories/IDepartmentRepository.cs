using Domain.Entities;

namespace Domain.Contracts.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<Department?> GetByNameAsync(string name, bool trackChanges = true, CancellationToken cancellationToken = default);
}

