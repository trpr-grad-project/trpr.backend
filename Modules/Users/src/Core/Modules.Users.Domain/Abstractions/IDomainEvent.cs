namespace Modules.Users.Domain.Abstractions;

public interface IDomainEvent
{
    public Guid Id { get; }
    public DateTime CreatedOnUtc { get; }
}
