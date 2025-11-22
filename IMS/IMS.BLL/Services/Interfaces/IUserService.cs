using Auth0.ManagementApi;
using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Enums;
using Shared.Pagination;

namespace IMS.BLL.Services.Interfaces;

public interface IUserService : IService<UserModel, User>
{
    public Task<PagedList<UserModel>> GetUsersAsync(PaginationParameters paginationParameters, 
        UserFilteringParameters filter, UserSortingParameter sorter,CancellationToken cancellationToken = default);

    public Task<UserModel> CreateAsync(UserModel model, string connection, CancellationToken cancellationToken = default);
}
