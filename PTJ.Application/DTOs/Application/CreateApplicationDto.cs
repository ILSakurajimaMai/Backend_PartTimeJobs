namespace PTJ.Application.DTOs.Application;

public class CreateApplicationDto
{
    public int JobPostId { get; set; }
    public int? ProfileId { get; set; }
    public string? CoverLetter { get; set; }
    public string? ResumeUrl { get; set; }
}
