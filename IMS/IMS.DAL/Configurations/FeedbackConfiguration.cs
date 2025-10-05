using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.DAL.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasOne(f => f.Task)
                .WithMany()
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Mentor)
                .WithMany()
                .HasForeignKey(f => f.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Intern)
                .WithMany()
                .HasForeignKey(f => f.InternId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
