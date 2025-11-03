using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL.Repositories;

public class InternshipRepository(ImsDbContext context, IInternshipFilterBuilder filterBuilder) : Repository<Internship>(context), IInternshipRepository
{
    private readonly DbSet<Internship> _internships = context.Set<Internship>();

    public async override Task<Internship?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default)
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
}
