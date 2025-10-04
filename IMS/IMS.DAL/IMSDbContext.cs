using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL
{
    public class IMSDbContext : DbContext
    {
        public IMSDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<Entities.Task> Tasks { get; set; } = null!;
        public DbSet<FeedBack> FeedBacks { get; set; } = null!;
        public DbSet<Internship> Internships { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Board>()
                .HasOne(b => b.CreatedBy)
                .WithMany()
                .HasForeignKey(b => b.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Board>()
                .HasOne(b => b.CreatedTo)
                .WithOne()
                .HasForeignKey<Board>(b => b.CreatedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Task>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeedBack>()
                .HasOne(f => f.Task)
                .WithMany()
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeedBack>()
                .HasOne(f => f.Mentor)
                .WithMany()
                .HasForeignKey(f => f.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeedBack>()
                .HasOne(f => f.Intern)
                .WithMany()
                .HasForeignKey(f => f.InternId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Internship>()
                .HasOne(i => i.Intern)
                .WithOne()
                .HasForeignKey<Internship>(i => i.InternId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Internship>()
                .HasOne(i => i.Mentor)
                .WithMany()
                .HasForeignKey(i => i.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Internship>()
                .HasOne(i => i.HRM)
                .WithMany()
                .HasForeignKey(i => i.HRMId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
