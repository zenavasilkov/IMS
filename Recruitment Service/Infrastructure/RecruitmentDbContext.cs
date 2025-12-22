using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class RecruitmentDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder) =>
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
}
 