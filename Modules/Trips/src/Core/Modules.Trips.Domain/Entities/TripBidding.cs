using Modules.Trips.Domain.Abstractions;
using Modules.Trips.Domain.Events;

namespace Modules.Trips.Domain.Entities
{
    public class TripBidding : Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TripId { get; set; }
        public Guid GuideId { get; set; }
        public double ProposedPrice { get; set; }
        public string? ProposalMessage { get; set; }
        public Trip Trip { get; set; } = default!;
        public User Guide { get; set; } = default!;
        public static TripBidding Create(double proposedPrice, string? proposalMessage, Trip trip, User guide)
        {
            var bidding = new TripBidding
            {
                Id = Guid.NewGuid(),
                TripId = trip.Id,
                GuideId = guide.Id,
                ProposedPrice = proposedPrice,
                ProposalMessage = proposalMessage,
                Trip = trip,
                Guide = guide
            };
            bidding.RaiseDomainEvent(new BiddingPlacedDomainEvent(bidding.Id));
            return bidding;
        }
    }
}
