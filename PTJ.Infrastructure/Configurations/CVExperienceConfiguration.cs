using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTJ.Domain.Entities;

namespace PTJ.Infrastructure.Configurations;

public class CVExperienceConfiguration : IEntityTypeConfiguration<CVExperience>
{
    public void Configure(EntityTypeBuilder<CVExperience> builder)
    {
        builder.ToTable("CVExperiences", "seeker");

        builder.HasKey(pe => pe.Id);

        builder.Property(pe => pe.CompanyName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pe => pe.Position)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pe => pe.Description)
            .HasMaxLength(2000);

        builder.HasIndex(pe => pe.ProfileId);

        builder.HasOne(pe => pe.CV)
            .WithMany(p => p.Experiences)
            .HasForeignKey(pe => pe.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
