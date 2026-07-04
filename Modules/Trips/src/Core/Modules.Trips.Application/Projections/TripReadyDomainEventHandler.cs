using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.DomainEvents;
using Common.Application.Exceptions;
using Modules.Trips.Application.Interfaces;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Projections
{
    public class TripReadyDomainEventHandler(ITripHubSender tripHubSender, RepositoryFactory repositoryFactory) : IDomainEventHandler<TripReadyDomainEvent>
    {
        public async Task HandleAsync(TripReadyDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var trip = repositoryFactory
                .Repository<Trip>()
                .GetQueryable()
                .FirstOrDefault(x => x.Id == domainEvent.TripId) ?? throw new NotFoundException("Trip.NotFound");

            await tripHubSender.SendTripUpdateAsync(trip, domainEvent.OldStatus);

        }
    }
}