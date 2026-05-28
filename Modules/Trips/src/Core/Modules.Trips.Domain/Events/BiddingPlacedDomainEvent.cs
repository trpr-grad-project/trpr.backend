using Common.Domain;

namespace Modules.Trips.Domain.Events;

public class BiddingPlacedDomainEvent(Guid tripBiddingId) : DomainEvent
{
    public Guid TripBiddingId { get; } = tripBiddingId;
}