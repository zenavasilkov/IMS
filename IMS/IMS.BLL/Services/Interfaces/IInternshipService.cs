using IMS.BLL.Models;
using IMS.DAL.Entities;

namespace IMS.BLL.Services.Interfaces;

public interface IInternshipService : IService<InternshipModel, Internship>
{
    Task<InternshipModel> CreateInternshipAsync(InternshipModel model, CancellationToken cancellationToken = default);
}
