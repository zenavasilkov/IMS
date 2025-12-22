using Domain.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

internal class InterviewRepository(IGenericRepository<Interview> repository, RecruitmentDbContext context) : IInterviewRepository
{
    public Task<Interview> CreateAsync(Interview entity, CancellationToken cancellationToken = default) =>
        repository.CreateAsync(entity, cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(id, cancellationToken);

    public async Task<PagedList<Interview>> GetInterviewsByCandidateId(Guid id, PaginationParameters paginationParameters,
        bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var interviews = await repository.GetByConditionAsync(i => i.CandidateId == id, paginationParameters, trackChanges, cancellationToken);

        return interviews;
    }

    public async Task<PagedList<Interview>> GetByConditionAsync(Expression<Func<Interview, bool>> expression,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Interview>()
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.Department)
            .Where(expression)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        query = trackChanges ? query : query.AsNoTracking();

        var list = await query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(cancellationToken);

        var pagedList = new PagedList<Interview>(list, paginationParameters.PageNumber, paginationParameters.PageSize, totalCount);

        return pagedList;
    }

    public async Task<Interview?> GetByIdAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Interview>().AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();

        var interview = await query
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.Department)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        return interview;
    }

    public Task<Interview> UpdateAsync(Interview entity, CancellationToken cancellationToken = default) =>
        repository.UpdateAsync(entity, cancellationToken);
}
