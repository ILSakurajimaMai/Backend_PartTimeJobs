using PTJ.Domain.Common;

namespace PTJ.Domain.Entities;

public class AIChatMessage : BaseAuditableEntity
{
    public int SessionId { get; set; }
    public string Role { get; set; } = string.Empty; // "user" or "assistant"
    public string Content { get; set; } = string.Empty;
    public int TokenCount { get; set; } // Optional approximation

    public virtual AIChatSession Session { get; set; } = null!;
}
