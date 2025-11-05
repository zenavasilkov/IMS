using IMS.DAL.Entities;
using Shared.Enums;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<PagedList<User>> GetAllAsync(
           PaginationParameters paginationParameters,
           UserFilteringParameters filter,
           UserSortingParameter sorter,
           bool trackChanges = false,
           CancellationToken cancellationToken = default);
    }
}
