using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Events;

public class UserCreatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserCreatedDomainEvent(Guid userId)
    {
        UserId = userId;
    }
}
