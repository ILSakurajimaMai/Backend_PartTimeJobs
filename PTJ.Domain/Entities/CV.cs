using PTJ.Domain.Common;
using PTJ.Domain.Enums;

namespace PTJ.Domain.Entities;

public class CV : BaseAuditableEntity
{
    public int UserId { get; set; }
    public int? ProfileId { get; set; }
    public string? Title { get; set; }
    public string? TargetPosition { get; set; }
    public bool IsDefault { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? StudentId { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    public decimal? GPA { get; set; }
    public int? YearOfStudy { get; set; }
    public DateTime? ExpectedGraduationDate { get; set; }
    public string? ResumeUrl { get; set; }
    public string? Bio { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Profile? Profile { get; set; }
    public virtual ICollection<CVSkill> Skills { get; set; } = new List<CVSkill>();
    public virtual ICollection<CVExperience> Experiences { get; set; } = new List<CVExperience>();
    public virtual ICollection<CVEducation> Educations { get; set; } = new List<CVEducation>();
    public virtual ICollection<CVCertificate> Certificates { get; set; } = new List<CVCertificate>();
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
