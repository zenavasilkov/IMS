using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 
using Shared.Enums;

namespace IMS.DAL.Repositories;

public class InternshipRepository(IMSDbContext context) : Repository<Internship>(context), IInternshipRepository
{
    private readonly DbSet<Internship> _internships = context.Set<Internship>();
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
