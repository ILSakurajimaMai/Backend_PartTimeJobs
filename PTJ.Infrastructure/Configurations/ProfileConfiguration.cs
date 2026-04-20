using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTJ.Domain.Entities;

namespace PTJ.Infrastructure.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles", "seeker");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(255);

        builder.Property(p => p.TargetPosition)
            .HasMaxLength(255);

        builder.Property(p => p.FullName)
            .HasMaxLength(255);

        builder.Property(p => p.Email)
            .HasMaxLength(255);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(p => p.StudentId)
            .HasMaxLength(50);

        builder.Property(p => p.University)
            .HasMaxLength(255);

        builder.Property(p => p.Major)
            .HasMaxLength(255);

        builder.Property(p => p.GPA)
            .HasPrecision(3, 2);

        builder.Property(p => p.ResumeUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Bio)
            .HasMaxLength(2000);

        builder.Property(p => p.LinkedInUrl)
            .HasMaxLength(255);

        builder.Property(p => p.GitHubUrl)
            .HasMaxLength(255);

        builder.HasIndex(p => p.UserId);

        builder.HasIndex(p => new { p.UserId, p.IsDefault })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0 AND [IsDefault] = 1");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
