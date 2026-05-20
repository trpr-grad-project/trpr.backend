using Modules.Users.Domain.Abstractions;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Domain.Entities;

public class SupportRequest : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SupportStatus Status { get; set; } = SupportStatus.Unread;
    public virtual User user { get; set; } = default!;

    public static SupportRequest Create(Guid userId, string subject, string description)
    {
        return new SupportRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Subject = subject,
            Description = description,
            CreatedAtUTC = DateTime.UtcNow,
            Status = SupportStatus.Unread
        };
    }
}
