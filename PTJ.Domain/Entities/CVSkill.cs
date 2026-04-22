using PTJ.Domain.Common;

namespace PTJ.Domain.Entities;

public class CVSkill : BaseEntity
{
    public int ProfileId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public int? ProficiencyLevel { get; set; }
    public int? YearsOfExperience { get; set; }

    // Navigation properties
    public virtual CV CV { get; set; } = null!;
}
