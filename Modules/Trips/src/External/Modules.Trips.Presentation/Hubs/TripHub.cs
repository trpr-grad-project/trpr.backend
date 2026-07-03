using Common.Presentation.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.ValueObjects;

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
                (x.Trip.StartDate == DateOnly.FromDateTime(DateTime.UtcNow)) &&
                (x.Trip.Status == TripStatus.Started || x.Trip.Status == TripStatus.Ready)
            )
            .Select(x => x.Trip)
            .ToListAsync();
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
                Status = x.Status.ToString()
            });
            foreach (var trip in trips)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, trip.Id.ToString());
            }
            await Clients.Caller.SendAsync("TripsToday", tripCast);
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {

        }
    }
}