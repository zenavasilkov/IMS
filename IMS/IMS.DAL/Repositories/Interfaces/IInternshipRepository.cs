using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.DAL.Repositories.Interfaces;

public interface IInternshipRepository : IRepository<Internship>
{
    Task<List<Internship>> GetInternshipsByStatusAsync(InternshipStatus status,CancellationToken cancellationToken = default);

    Task<List<Internship>> GetInternshipsByMentorIdAsync(Guid mentorId, CancellationToken cancellationToken = default);

    Task<List<Internship>> GetInternshipsByHumanResourcesManagerIdAsync(Guid hrManagerId, CancellationToken cancellationToken = default);

    Task<Internship> GetInternshipsByInternIdAsync(Guid internId, CancellationToken cancellationToken = default);
}
