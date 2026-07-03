
using Modules.Notifications.Domain.Abstractions;
using Modules.Notifications.Domain.DomainEvents;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Domain.Entities;

public class Notification : Entity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
    public ContentType ContentType { get; set; } = ContentType.Pure;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public static Notification Create(
        string title,
        string message,
        User user)
    {
        var not = new Notification
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            Message = message,
            ContentType = ContentType.Pure,
            UserId = user.Id,
            SequenceNumber = user.LatestSequenceNumber
        };
        user.LatestSequenceNumber++;
        not.RaiseDomainEvent(new NotificationCreatedDomainEvent
        {
            Id = not.Id
        });
        return not;
    }
}
