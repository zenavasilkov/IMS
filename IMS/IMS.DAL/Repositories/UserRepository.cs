using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Pagination;

namespace IMS.DAL.Repositories
{
    public class UserRepository(IMSDbContext context, IUserFilterBuilder filterBuilder) 
        : Repository<User>(context), IUserRepository
    {
        private readonly DbSet<User> _users = context.Set<User>();
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var exists = await _users.AnyAsync(u => u.Email == email, cancellationToken);

            return exists;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var query = _users
                .AsNoTracking()
                .Include(u => u.Internships)
                .AsQueryable();

            var user = await filterBuilder
                .WithEmail(email)
                .Build(query)
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        public async Task<PagedList<User>> GetAllAsync(
            PaginationParameters paginationParameters,
            UserFilteringParameters filter,
            UserSortingParameter sorter,
            bool trackChanges = false,
            CancellationToken cancellationToken = default)
        {
            var query = _users.AsQueryable();

            query = trackChanges ? query : query.AsNoTracking();

            if(filter.Role != null) query = query.Where(u => u.Role == filter.Role);
            if(filter.FirstName != null) query = query.Where(u => u.Firstname == filter.FirstName);
            if(filter.LastName != null) query = query.Where(u => u.Lastname == filter.LastName);

            query = sorter switch
            {
                UserSortingParameter.AscendingId => query.OrderBy(u => u.Id),
                UserSortingParameter.AscendingFirstName => query.OrderBy(u => u.Firstname),
                UserSortingParameter.AscendingLastName => query.OrderBy(u => u.Lastname),
                UserSortingParameter.AscendingRole => query.OrderBy(u => u.Role),
                UserSortingParameter.DescendingId => query.OrderByDescending(u => u.Id),
                UserSortingParameter.DescendingFirstName => query.OrderByDescending(u => u.Firstname),
                UserSortingParameter.DescendingLastName => query.OrderByDescending(u => u.Lastname),
                UserSortingParameter.DescendingRole => query.OrderByDescending(u => u.Role),
                _ => query
            };

            var users = await query
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedList = new PagedList<User>(users, paginationParameters.PageNumber, 
                paginationParameters.PageSize, totalCount);

            return pagedList;
        }
    }
}
