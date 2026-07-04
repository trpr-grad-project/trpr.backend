using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.DomainEvents;
using Common.Application.Exceptions;
using Modules.Notifications.Contracts.Contracts;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Notifications.Contracts.Dtos;
using Modules.Trips.Domain.Events;

namespace Modules.Trips.Application.Projections
{
    public class TripStatusUpdatedDomainEventHandler(INotifiyContract notifiyContract, RepositoryFactory repositoryFactory) : IDomainEventHandler<TripStatusUpdatedDomainEVent>
    {
        public async Task HandleAsync(TripStatusUpdatedDomainEVent domainEvent, CancellationToken cancellationToken = default)
        {
            var trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(x => x.Id == domainEvent.TripId) ?? throw new NotFoundException("Trip.NotFound");

            await notifiyContract.NotifyUsersAsync(new NotifyUsersRequestDto(
                    $"Trip {trip.Title} Status Updated",
                    $"The status of your trip \"{trip.Title}\" has changed from {domainEvent.OldTripStatus} to {trip.Status}.",
                    [trip.UserId]
                )
            , cancellationToken);
        }
    }
}