using Domain.Entities;

namespace Domain.Contracts.Repositories;

public interface IGeneralRepository<T> : IGenericReadOnlyRepository<T> where T : Entity
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(CancellationToken cancellationToken = default);
}
