using Microsoft.AspNetCore.SignalR;
using Modules.Trips.Application.Dtos.Hub;
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
            var tripCast = trips.Select(x => new TripHubDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                StartDate = x.StartDate,
                ActualDuration = x.ActualDuration,
                ExpectedDuration = x.ExpectedDuration,
                Price = x.Price,
                Images = x.Images,
                MaxParticipantsCount = x.MaxParticipantsCount,
                GuideId = x.GuideId,
                Status = x.Status.ToString()
            }).ToList();
            await hubContext.Clients.Group(trip.Id.ToString()).SendAsync("TripStatusUpdated", tripCast.FirstOrDefault(), cancellationToken);
        }
    }
}