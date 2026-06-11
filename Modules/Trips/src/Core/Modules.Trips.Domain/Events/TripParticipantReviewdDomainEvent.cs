using Common.Domain;

namespace Modules.Trips.Domain.Events
{
    public class TripParticipantReviewdDomainEvent(Guid tripId, Guid userId, double? rating, string? review) : DomainEvent
    {
        public Guid TripId { get; set; } = tripId;
        public Guid UserId { get; set; } = userId;
        public double? Rating { get; set; } = rating;
        public string? Review { get; set; } = review;
    }
}