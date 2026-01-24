namespace Modules.Notifications.Domain.Abstractions;

public interface IDomainEvent
{
    public Guid Id { get; }
    public DateTime CreatedOnUtc { get; }
}
