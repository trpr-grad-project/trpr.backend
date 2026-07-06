using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities;

public class TripReview : Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TripId { get; set; }
    public Guid ReviewerId { get; set; }
    public Guid RevieweeId { get; set; }
    public double? Rating { get; set; }
    public string? Review { get; set; }


    public virtual Trip Trip { get; set; } = default!;
    public virtual User Reviewer { get; set; } = default!;
    public virtual User Reviewee { get; set; } = default!;

    public string ReviewerName => Reviewer?.UserName ?? string.Empty;
    public string RevieweeName => Reviewee?.UserName ?? string.Empty;

    public static TripReview Create(
        Guid tripId,
        Guid reviewerId,
        Guid revieweeId,
        double? rating,
        string? review)
    {
        return new TripReview
        {
            TripId = tripId,
            ReviewerId = reviewerId,
            RevieweeId = revieweeId,
            Rating = rating,
            Review = review,
        };
    }
}
