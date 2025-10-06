using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.DAL.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasOne(f => f.Task)
            .WithMany()
            .HasForeignKey(f => f.TaskId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.SendedBy)
            .WithMany()
            .HasForeignKey(f => f.SendedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.AddressedTo)
            .WithMany()
            .HasForeignKey(f => f.AddressedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
