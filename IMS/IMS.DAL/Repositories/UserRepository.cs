using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums; 

namespace IMS.DAL.Repositories
{
    public class UserRepository(IMSDbContext context) : Repository<User>(context), IUserRepository
    {
        private readonly DbSet<User> _users = context.Set<User>();
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var exists = await _users.AnyAsync(u => u.Email == email, cancellationToken);

            return exists;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            return user;
        }

        public Task<List<User>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            var user = _users
                .AsNoTracking()
                .Where(u => u.Role == role)
                .ToListAsync(cancellationToken);

            return user;
        }
    }
}
