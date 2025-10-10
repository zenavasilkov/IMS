using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace IMS.DAL.Configurations;

public class InternshipConfiguration : IEntityTypeConfiguration<Internship>
{
    public void Configure(EntityTypeBuilder<Internship> builder)
    { 
        builder.HasOne(i => i.Intern)
            .WithMany()
            .HasForeignKey(i => i.InternId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Mentor)
            .WithMany()
            .HasForeignKey(i => i.MentorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.HumanResourcesManager)
            .WithMany()
            .HasForeignKey(i => i.HumanResourcesManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
