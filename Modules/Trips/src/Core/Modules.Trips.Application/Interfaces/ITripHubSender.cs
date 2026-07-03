using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Interfaces
{
    public interface ITripHubSender
    {
        public Task SendTripUpdateAsync(Trip trip, TripStatus oldStatus, CancellationToken cancellationToken = default);
    }
}