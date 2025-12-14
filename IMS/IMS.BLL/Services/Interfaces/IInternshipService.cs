using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.BLL.Services.Interfaces;

public interface IInternshipService : IService<InternshipModel, Internship>
{
    Task<InternshipModel> CreateInternshipAsync(InternshipModel model, CancellationToken cancellationToken = default);
    
    Task<PagedList<InternshipModel>> GetAllInternshipsAsync(
        PaginationParameters paginationParameters,
        InternshipFilteringParameters filter,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);
}
