using Modules.Trips.Domain.Entities;
using Modules.Trips.Application.Services;
using Modules.Trips.Infrastructure.Clients;
using Modules.Trips.Infrastructure.Dtos.Requests;
using Modules.Trips.Infrastructure.Mappers;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Infrastructure.Dtos.Responses;
using Microsoft.AspNetCore.Localization;
namespace Modules.Trips.Infrastructure.Services;

public class AiTripSuggestionService(
    TripPlanClient tripPlanClient
) : IAiTripSuggestionService
{
    public async Task<List<List<SuggestedPlaceDto>>> GenerateTripPlan(DateTime startDate, Theme theme, ICollection<PlaceDto> places, int numberOfDays)
    {
        var tripPlanRequest = new TripPlanRequestDto
        {
            Theme = theme.Name,
            Places = [.. places.Select(p => p.ToTripPlanPlaceDto())],
            NumberOfDays = numberOfDays,
            DateFrom = startDate
        };
        TripPlanResponseDto tripPlanResponseDto = await tripPlanClient.GeneratePlanAsync(tripPlanRequest);
        return [.. tripPlanResponseDto
            .ActiveTrip
            .Itinerary
            .DailyItinerary
            .Select(
                x => x.Activities.Select(
                        y => new SuggestedPlaceDto
                        {
                            Id = y.Place.Id,
                            Title = y.Place.Title,
                            Description = y.Place.Description,
                            AverageVisitTime = y.EstimatedDurationHours,
                            Latitude = y.Place.Location.Latitude,
                            Longitude = y.Place.Location.Longitude,
                        }
                    ).ToList()
            )];
    }
}
