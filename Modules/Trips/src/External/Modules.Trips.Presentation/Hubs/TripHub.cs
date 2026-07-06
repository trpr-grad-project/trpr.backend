using Common.Presentation.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Dtos.Hub;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.ValueObjects;
using Rebus.Sagas.Idempotent;

namespace Modules.Trips.Presentation.Hubs
{
    public class TripHub(RepositoryFactory repositoryFactory) : Hub
    {
        public Guid UserId => Context.User!.GetUserId();
        public async override Task OnConnectedAsync()
        {
            var trips = await repositoryFactory.Repository<TripParticipant>()
            .GetQueryable()
            .Include(x => x.Trip)
            .Where(x =>
                (x.UserId == UserId) &&
                (x.Trip.StartDate == DateTime.UtcNow) &&
                (x.Trip.Status == TripStatus.Started || x.Trip.Status == TripStatus.Ready)
            )
            .Select(x => x.Trip)
            .ToListAsync();
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
            foreach (var trip in trips)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, trip.Id.ToString());
            }
            await Clients.Caller.SendAsync("TripsToday", tripCast);
            await base.OnConnectedAsync();
        }

        public async Task UpdateLocation(Guid tripId, double latitude, double longitude)
        {
            await Clients.OthersInGroup(tripId.ToString())
                .SendAsync("LocationUpdated", new
                {
                    UserId,
                    TripId = tripId,
                    Latitude = latitude,
                    Longitude = longitude
                });
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {

        }
    }
}