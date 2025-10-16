using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 
using Shared.Enums;

namespace IMS.DAL.Repositories;

public class InternshipRepository(IMSDbContext context) : Repository<Internship>(context), IInternshipRepository
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

    public async Task<List<Internship>> GetActiveInternshipsAsync(CancellationToken cancellationToken = default)
    {
        var internships = await _internships
            .AsNoTracking()
            .Where(i => i.Status == InternshipStatus.Ongoing)
            .ToListAsync(cancellationToken);

        return internships;
    }

    public async Task<List<Internship>> GetInternshipsByHumanResourcesManagerIdAsync(Guid hrManagerId, CancellationToken cancellationToken = default)
    {
        var internships = await _internships
            .AsNoTracking()
            .Where(i => i.HumanResourcesManagerId == hrManagerId)
            .ToListAsync(cancellationToken);

        return internships;
    }

    public async Task<Internship> GetInternshipsByInternIdAsync(Guid internId, CancellationToken cancellationToken = default)
    {
        var intership = await _internships
            .FirstOrDefaultAsync(i => i.InternId == internId, cancellationToken);

        return intership!;
    }

    public async Task<List<Internship>> GetInternshipsByMentorIdAsync(Guid mentorId, CancellationToken cancellationToken = default)
    {
        var internships = await _internships
            .AsNoTracking()
            .Where(i => i.MentorId == mentorId)
            .ToListAsync(cancellationToken);

        return internships;
    }
}
