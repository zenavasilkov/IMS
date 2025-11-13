using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Infrastructure.Constants.TableNames;

namespace Infrastructure.Configuration;

internal class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable(Candidates);

        builder.HasKey(e => e.Id);

        builder.OwnsOne(e => e.FullName, fullName =>
        {
            fullName.Property(f => f.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired();

            fullName.Property(f => f.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired();

            fullName.Property(f => f.Patronymic)
                .HasColumnName("Patronymic")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired(false);
        });
    }
}
