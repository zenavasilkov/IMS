using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Infrastructure.Constants.TableNames;

namespace Infrastructure.Configuration;

internal class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.ToTable(Interviews);

        builder.HasKey(i => i.Id);

        builder
            .HasOne(i => i.Candidate)
            .WithMany()
            .HasForeignKey(i => i.CandidateId);

        builder
            .HasOne(i => i.Interviewer)
            .WithMany()
            .HasForeignKey(i => i.InterviewerId);

        builder
            .HasOne(i => i.Department)
            .WithMany()
            .HasForeignKey(i => i.DepartmentId);
    }
}
