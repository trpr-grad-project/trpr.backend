using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;
using Microsoft.Win32;

namespace Modules.Trips.Domain.Events
{
    public class TripParticipantCreatedDomainEvent(Guid tripId, Guid userId) : DomainEvent
    {
        public Guid TripId { get; set; } = tripId;
        public Guid UserId { get; set; } = userId;
    }
}