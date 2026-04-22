using PTJ.Domain.Enums;

namespace PTJ.Application.DTOs.CV;

public class CVDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
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
    public List<CVSkillDto> Skills { get; set; } = new();
    public List<CVExperienceDto> Experiences { get; set; } = new();
    public List<CVEducationDto> Educations { get; set; } = new();
    public List<CVCertificateDto> Certificates { get; set; } = new();
}

public class CVSkillDto
{
    public int Id { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public int? ProficiencyLevel { get; set; }
    public int? YearsOfExperience { get; set; }
}

public class CVExperienceDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentlyWorking { get; set; }
}

public class CVEducationDto
{
    public int Id { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string? FieldOfStudy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? GPA { get; set; }
    public string? Description { get; set; }
}

public class CVCertificateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IssuingOrganization { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public string? CertificateFileUrl { get; set; }
}
