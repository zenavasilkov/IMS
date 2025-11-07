using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class RecruitmentDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Interview> Interviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecruitmentDbContext).Assembly);
    }
}
