using IMS.DAL.Entities;

namespace IMS.DAL.Repositories.Interfaces;

public interface IInternshipRepository : IRepository<Internship>
{
    Task<Internship> GetInternshipsByInternIdAsync(Guid internId, CancellationToken cancellationToken = default);
}
