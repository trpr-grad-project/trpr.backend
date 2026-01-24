using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Events;

public class TokenCreatedDomainEvent(Guid tokenId) : DomainEvent
{
    public Guid TokenId { get; } = tokenId;
}
