using Domain.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CandidateRepository(RecruitmentDbContext context, IGenericRepository<Candidate> repository) : ICandidateRepository
{
    public Task<Candidate> CreateAsync(Candidate entity, CancellationToken cancellationToken = default) =>
        repository.CreateAsync(entity, cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => repository.DeleteAsync(id, cancellationToken);

    public Task<PagedList<Candidate>> GetByConditionAsync(Expression<Func<Candidate, bool>> expression,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetByConditionAsync(expression, paginationParameters, trackChanges, cancellationToken);

    public async Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var candidate = await context.Candidates.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);

        return candidate;
    }

    public async Task<Candidate?> GetByEmailAsync(string email, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        var query = context.Candidates.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();

        var candidate = await query.FirstOrDefaultAsync(d => d.Email == email, cancellationToken);

        return candidate;
    }

    public Task<Candidate?> GetByIdAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public Task<Candidate> UpdateAsync(Candidate entity, CancellationToken cancellationToken = default) =>
        repository.UpdateAsync(entity, cancellationToken);
}
