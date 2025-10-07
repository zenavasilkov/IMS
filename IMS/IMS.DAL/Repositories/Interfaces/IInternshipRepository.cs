using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.DAL.Entities;

namespace IMS.DAL.Repositories.Interfaces;

public interface IInternshipRepository : IRepository<Internship>
{
    Task<List<Internship>> GetActiveInternshipsAsync(CancellationToken cancellationToken = default);

    Task<List<Internship>> GetInternshipsByMentorAsync(User mentor, CancellationToken cancellationToken = default);

    Task<List<Internship>> GetInternshipsByHumanResourcesManagerAsync(User hrManager, CancellationToken cancellationToken = default);

    Task<Internship> GetInternshipsByInternAsync(User intern, CancellationToken cancellationToken = default);
}
