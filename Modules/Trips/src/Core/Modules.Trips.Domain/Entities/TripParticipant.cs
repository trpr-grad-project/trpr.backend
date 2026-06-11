using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Trips.Domain.Abstractions;
using Modules.Trips.Domain.Events;

namespace Modules.Trips.Domain.Entities
{
    public class TripParticipant : Entity
    {
        // TODO migrate and make endpoints for reviews and ratings
        // TODO make join endpoint
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
        public virtual Trip Trip { get; set; } = default!;
        public virtual User User { get; set; } = default!;
        public double? Rating { get; set; }
        public string? Review { get; set; }

        public static TripParticipant Create(Guid tripId, Guid userId)
        {
            var tripParticipant = new TripParticipant
            {
                UserId = userId,
                TripId = tripId
            };
            tripParticipant.RaiseDomainEvent(new TripParticipantCreatedDomainEvent(tripId, userId));
            return tripParticipant;
        }

        public void MakeReview(double? rate, string? review)
        {
            if (rate.HasValue)
                this.Rating = rate;
            if (!string.IsNullOrEmpty(review))
                this.Review = review;

            RaiseDomainEvent(
                new TripParticipantReviewdDomainEvent(
                    this.TripId,
                    this.UserId,
                    rate,
                    review));
        }
    }
}
