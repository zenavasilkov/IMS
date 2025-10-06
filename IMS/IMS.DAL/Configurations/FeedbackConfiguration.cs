using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.DAL.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasOne(f => f.Ticket)
            .WithMany()
            .HasForeignKey(f => f.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.SentBy)
            .WithMany()
            .HasForeignKey(f => f.SentById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.AddressedTo)
            .WithMany()
            .HasForeignKey(f => f.AddressedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
