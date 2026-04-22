namespace PTJ.Application.DTOs.Profile;

public class ProfileDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

public class UpdateProfileDto
{
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}
