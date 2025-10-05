using Microsoft.EntityFrameworkCore;
using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace IMS.DAL.Configurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.HasOne(b => b.CreatedBy)
                .WithMany()
                .HasForeignKey(b => b.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.CreatedTo)
                .WithOne()
                .HasForeignKey<Board>(b => b.CreatedToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
