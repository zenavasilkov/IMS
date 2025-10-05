using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL
{
    public class IMSDbContext(DbContextOptions options) : DbContext(options)
    { 
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<Entities.Task> Tasks { get; set; } = null!;
        public DbSet<Feedback> FeedBacks { get; set; } = null!;
        public DbSet<Internship> Internships { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IMSDbContext).Assembly);
        }
    }
}
