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

        builder.Property(p => p.FullName)
            .HasMaxLength(255);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(p => p.Email)
            .HasMaxLength(255);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
