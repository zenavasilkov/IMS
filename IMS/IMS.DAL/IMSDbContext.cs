using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL;

public class IMSDbContext(DbContextOptions options) : DbContext(options)
{ 
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Feedback> FeedBacks { get; set; }
    public DbSet<Internship> Internships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IMSDbContext).Assembly);
    }
}
