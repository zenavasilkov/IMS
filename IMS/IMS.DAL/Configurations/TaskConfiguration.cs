using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace IMS.DAL.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Entities.Ticket>
{
    public void Configure(EntityTypeBuilder<Entities.Ticket> builder)
    { 
        builder.HasOne(t => t.Board)
            .WithMany(b => b.Tasks)
            .HasForeignKey(t => t.BoardId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
