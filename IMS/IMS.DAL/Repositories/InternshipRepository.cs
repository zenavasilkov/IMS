using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using System.Linq.Expressions;

namespace IMS.DAL.Repositories;

public class InternshipRepository(ImsDbContext context, IInternshipFilterBuilder filterBuilder,
    IRepository<Internship> repository) : IInternshipRepository
{
    private readonly DbSet<Internship> _internships = context.Set<Internship>();

    public Task<Internship> CreateAsync(Internship entity, CancellationToken cancellationToken = default) =>
        repository.CreateAsync(entity, cancellationToken);

    public Task DeleteAsync(Internship entity, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(entity, cancellationToken);

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

    public async Task<Internship> GetInternshipsByInternIdAsync(Guid internId, CancellationToken cancellationToken = default)
    {
        var query = _internships
            .AsNoTracking()
            .Include(i => i.Intern)
            .Include(i => i.Mentor)
            .Include(i => i.HumanResourcesManager)
            .AsQueryable();

        var internships = await filterBuilder
            .WithIntern(internId)
            .Build(query)
            .FirstAsync(cancellationToken);

        return internships;
    }

    public Task<PagedList<Internship>> GetPagedAsync(Expression<Func<Internship, bool>>? predicate,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<Internship> UpdateAsync(Internship entity, CancellationToken cancellationToken = default) =>
        repository.UpdateAsync(entity, cancellationToken);
}
