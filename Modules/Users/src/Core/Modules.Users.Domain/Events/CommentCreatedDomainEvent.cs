using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Events;

public class CommentCreatedDomainEvent : DomainEvent
{
    public string CommentId { get; }

    public CommentCreatedDomainEvent(string commentId)
    {
        CommentId = commentId;
    }
}
