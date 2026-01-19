using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Events;

public class PostDeletedDomainEvent : DomainEvent
{
    public Guid PostId { get; }
    public PostDeletedDomainEvent(Guid postId)
    {
        PostId = postId;
    }
}
