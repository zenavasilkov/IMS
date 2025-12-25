using IMS.DAL.Entities;
using IMS.DAL.Outbox;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL;

public class ImsDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Internship> Internships { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImsDbContext).Assembly);
    }
}
