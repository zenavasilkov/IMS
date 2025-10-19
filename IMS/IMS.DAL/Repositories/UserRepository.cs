using IMS.DAL.Builders;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums; 

namespace IMS.DAL.Repositories
{
    public class UserRepository(IMSDbContext context, IUserFilterBuilder filterBuilder) : Repository<User>(context), IUserRepository
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

        public async Task<List<User>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            var query = _users
                .AsNoTracking()
                .Include(u => u.Internships)
                .AsQueryable();

            var users = await filterBuilder
                .WithRole(role)
                .Build(query)
                .OrderBy(u => u.Id)
                .ToListAsync(cancellationToken);

            return users;
        }
    }
}
