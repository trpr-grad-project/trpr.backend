using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Events;

public class PostCreatedDomainEvent : DomainEvent
{
    public Guid PostId { get; }
    public PostCreatedDomainEvent(Guid postId)
    {
        PostId = postId;
    }
}
