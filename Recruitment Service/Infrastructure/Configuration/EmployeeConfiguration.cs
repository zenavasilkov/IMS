using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DepartmentId);

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
