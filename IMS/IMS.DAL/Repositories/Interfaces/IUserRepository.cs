using IMS.DAL.Entities;
using Shared.Enums;
using Shared.Pagination;

namespace IMS.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

        public Task<PagedList<User>> GetAllAsync(
           PaginationParameters paginationParameters,
           UserFilteringParameters filter,
           UserSortingParameter sorter,
           bool trackChanges = false,
           CancellationToken cancellationToken = default);
    }
}
