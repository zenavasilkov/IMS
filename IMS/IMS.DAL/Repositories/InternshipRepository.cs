using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 
using Shared.Enums;

namespace IMS.DAL.Repositories;

public class InternshipRepository(IMSDbContext context, IInternshipFilterBuilder filterBuilder) : Repository<Internship>(context), IInternshipRepository
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

    public async Task<List<Internship>> GetInternshipsByStatusAsync(InternshipStatus status, CancellationToken cancellationToken = default)
    {
        var query = _internships
            .AsNoTracking()
            .Include(i => i.Intern)
            .Include(i => i.Mentor)
            .Include(i => i.HumanResourcesManager)
            .AsQueryable();

        var internships = await filterBuilder
            .WithStatus(status)
            .Build(query)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);

        return internships;
    }

    public async Task<List<Internship>> GetInternshipsByHumanResourcesManagerIdAsync(Guid hrManagerId, CancellationToken cancellationToken = default)
    {
        var query = _internships
            .AsNoTracking()
            .Include(i => i.Intern)
            .Include(i => i.Mentor)
            .Include(i => i.HumanResourcesManager)
            .AsQueryable();

        var internships = await filterBuilder
            .WithHumanResourcesManager(hrManagerId)
            .Build(query)
            .ToListAsync(cancellationToken);

        return internships;
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

    public async Task<List<Internship>> GetInternshipsByMentorIdAsync(Guid mentorId, CancellationToken cancellationToken = default)
    {
        var query = _internships
            .AsNoTracking()
            .Include(i => i.Intern)
            .Include(i => i.Mentor)
            .Include(i => i.HumanResourcesManager)
            .AsQueryable();

        var internships = await filterBuilder
            .WithMentor(mentorId)
            .Build(query)
            .ToListAsync(cancellationToken);

        return internships;
    }
}
