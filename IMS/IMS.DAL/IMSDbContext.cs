using IMS.DAL.Entities;
using IMS.DAL.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL;

public class IMSDbContext(DbContextOptions options, UpdateTimestampsInterceptor timestampsInterceptor) : DbContext(options)
{ 
    private readonly UpdateTimestampsInterceptor _timestampsInterceptor = timestampsInterceptor;

    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Internship> Internships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IMSDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_timestampsInterceptor);
    }
}
