using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Events;

public class PostRatingDomainEvent : DomainEvent
{
    public Guid PostId { get; }
    public byte Rating { get; }
    public bool IsUpdate { get; set; }
    public byte? OldRating { get; set; }

    public PostRatingDomainEvent(Guid postId, byte rating, bool isUpdate = false, byte? oldRating = null)
    {
        PostId = postId;
        Rating = rating;
        IsUpdate = isUpdate;
        OldRating = oldRating;
    }
}