using PTJ.Domain.Common;

namespace PTJ.Domain.Entities;

public class Profile : BaseAuditableEntity
{
    public int UserId { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CV> CVs { get; set; } = new List<CV>();
}
