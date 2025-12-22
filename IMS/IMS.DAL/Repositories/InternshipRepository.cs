using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using System.Linq.Expressions;
using Shared.Filters;

namespace IMS.DAL.Repositories;

public class InternshipRepository(ImsDbContext context, IRepository<Internship> repository) : IInternshipRepository
{
    private readonly DbSet<Internship> _internships = context.Set<Internship>();

    public Task<Internship> CreateAsync(Internship entity, CancellationToken cancellationToken = default) =>
        repository.CreateAsync(entity, cancellationToken);

    public Task DeleteAsync(Internship entity, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(entity, cancellationToken);

    public async Task<PagedList<Internship>> GetAllAsync(
        PaginationParameters paginationParameters,
        InternshipFilteringParameters filter,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        var query = _internships.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();
        query = ApplyFilters(query, filter);

        var internships = await query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedList = new PagedList<Internship>(internships, paginationParameters.PageNumber, 
            paginationParameters.PageSize, totalCount);

        return pagedList;
    }
    
    private static IQueryable<Internship> ApplyFilters(IQueryable<Internship> query, InternshipFilteringParameters filter)
    {
        query = new InternshipFilterBuilder()
            .WithIntern(filter.InternId)
            .WithMentor(filter.MentorId)
            .WithHumanResourcesManager(filter.HrManagerId)
            .StarterAfter(filter.StartedAfter)
            .StartedBefore(filter.StartedBefore)
            .WithStatus(filter.Status)
            .Build(query);

        return query;
    }

    public Task<List<Internship>> GetAllAsync(Expression<Func<Internship, bool>>? predicate = null,
        bool trackChanges = false, CancellationToken cancellationTokent = default) =>
        repository.GetAllAsync(predicate, trackChanges, cancellationTokent);

    public async Task<Internship?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var query = _internships.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();

        var internship = await query
            .Include(i => i.Intern)
            .Include(i => i.Mentor)
            .Include(i => i.HumanResourcesManager)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return internship;
    }

    public Task<PagedList<Internship>> GetPagedAsync(Expression<Func<Internship, bool>>? predicate,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<Internship> UpdateAsync(Internship entity, CancellationToken cancellationToken = default) =>
        repository.UpdateAsync(entity, cancellationToken);
}
