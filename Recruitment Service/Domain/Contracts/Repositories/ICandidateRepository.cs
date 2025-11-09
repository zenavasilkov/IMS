using Domain.Entities;

namespace Domain.Contracts.Repositories;

public interface ICandidateRepository : IGenericRepository<Candidate>
{
    Task<Candidate?> GetByEmailAsync(string email, bool trackChanges = true, CancellationToken cancellationToken = default);
}
