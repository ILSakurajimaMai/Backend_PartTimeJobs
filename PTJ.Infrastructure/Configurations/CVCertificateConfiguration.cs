using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTJ.Domain.Entities;

namespace PTJ.Infrastructure.Configurations;

public class CVCertificateConfiguration : IEntityTypeConfiguration<CVCertificate>
{
    public void Configure(EntityTypeBuilder<CVCertificate> builder)
    {
        builder.ToTable("CVCertificates", "seeker");

        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pc => pc.IssuingOrganization)
            .HasMaxLength(255);

        builder.Property(pc => pc.CredentialId)
            .HasMaxLength(100);

        builder.Property(pc => pc.CredentialUrl)
            .HasMaxLength(500);

        builder.Property(pc => pc.CertificateFileUrl)
            .HasMaxLength(500);

        builder.HasIndex(pc => pc.ProfileId);

        builder.HasOne(pc => pc.CV)
            .WithMany(p => p.Certificates)
            .HasForeignKey(pc => pc.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
