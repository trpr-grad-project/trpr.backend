using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities
{
    public class TripRating : Entity
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid ReviewerId { get; set; }
        public double? Rating { get; set; }
        public string? Review { get; set; }

        // Navigation properties
        public virtual Trip Trip { get; set; } = null!;
        public virtual User Reviewer { get; set; } = null!;

        public string ReviewerName => Reviewer?.FirstName + " " + Reviewer?.LastName ?? string.Empty;

        public static TripRating Create(Guid tripId, Guid reviewerId, double? rating, string? review)
        {
            return new TripRating
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                ReviewerId = reviewerId,
                Rating = rating,
                Review = review
            };
        }
    }
}
