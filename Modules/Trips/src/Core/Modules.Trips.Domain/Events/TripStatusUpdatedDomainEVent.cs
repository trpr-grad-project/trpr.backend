using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Domain.Events
{
    public class TripStatusUpdatedDomainEVent(Guid tripId, TripStatus oldStatus) : DomainEvent
    {
        public Guid TripId { get; set; } = tripId;
        public TripStatus OldTripStatus { get; set; } = oldStatus;
    }
}