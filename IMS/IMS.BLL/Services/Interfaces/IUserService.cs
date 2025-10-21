using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Enums;
using Shared.Pagination;

namespace IMS.BLL.Services.Interfaces;

public interface IUserService : IService<UserModel, User>
{    Task<List<UserModel>> GetUsersByRoleAsync(Role role, bool trackCanges = false, CancellationToken cancellationToken = default);

    public Task<PagedList<UserModel>> GetUsersAsync(PaginationParameters paginationParameters, 
        UserFilteringParameters filter, UserSortingParameter sorter,CancellationToken cancellationToken = default);
}
