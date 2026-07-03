using Microsoft.AspNetCore.SignalR;
using Modules.Trips.Application.Interfaces;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.ValueObjects;
using Modules.Trips.Presentation.Hubs;

namespace Modules.Trips.Infrastructure.Services
{
    public class TripHubSender(IHubContext<TripHub> hubContext) : ITripHubSender
    {
        public async Task SendTripUpdateAsync(Trip trip, TripStatus oldStatus, CancellationToken cancellationToken = default)
        {
            var trips = new[] { trip };
            var tripCast = trips.Select(x => new
            {
                x.Id,
                x.Title,
                x.Description,
                x.StartDate,
                x.ActualDuration,
                x.ExpectedDuration,
                x.Price,
                x.Images,
                x.Centroid,
                x.TripVisibility,
                x.MaxParticipantsCount,
                x.GuideId,
                Status = x.Status.ToString(),
                OldStatus = oldStatus.ToString(),
            });
            await hubContext.Clients.Group(trip.Id.ToString()).SendAsync("TripStatusUpdated", tripCast.FirstOrDefault(), cancellationToken);
        }
    }
}