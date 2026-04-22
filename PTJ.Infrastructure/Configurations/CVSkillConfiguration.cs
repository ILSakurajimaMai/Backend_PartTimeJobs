using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTJ.Domain.Entities;

namespace PTJ.Infrastructure.Configurations;

public class CVSkillConfiguration : IEntityTypeConfiguration<CVSkill>
{
    public void Configure(EntityTypeBuilder<CVSkill> builder)
    {
        builder.ToTable("CVSkills", "seeker");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.SkillName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(ps => ps.ProfileId);

        builder.HasOne(ps => ps.CV)
            .WithMany(p => p.Skills)
            .HasForeignKey(ps => ps.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
