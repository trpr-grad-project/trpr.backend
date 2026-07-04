using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Domain.Entities
{
    public class TripReadyDomainEvent(Guid tripId, TripStatus oldStatus) : DomainEvent
    {
        public Guid TripId { get; set; } = tripId;
        public TripStatus OldStatus { get; set; } = oldStatus;
    }
}