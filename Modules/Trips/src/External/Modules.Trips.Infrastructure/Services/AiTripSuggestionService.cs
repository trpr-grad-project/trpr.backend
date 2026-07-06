using Modules.Trips.Domain.Entities;
using Modules.Trips.Application.Services;
using Modules.Trips.Infrastructure.Clients;
using Modules.Trips.Infrastructure.Dtos.Requests;
using Modules.Trips.Infrastructure.Mappers;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Infrastructure.Dtos.Responses;
namespace Modules.Trips.Infrastructure.Services;

public class AiTripSuggestionService(
    TripPlanClient tripPlanClient
) : IAiTripSuggestionService
{
    public async Task<object> GenerateTripPlan(DateTime startDate, Theme theme, ICollection<PlaceDto> places, int numberOfDays)
    {
        var tripPlanRequest = new TripPlanRequestDto
        {
            Theme = theme.Name,
            Places = [.. places.Select(p => p.ToTripPlanPlaceDto())],
            NumberOfDays = numberOfDays,
            DateFrom = startDate
        };
        TripPlanResponseDto tripPlanResponseDto = await tripPlanClient.GeneratePlanAsync(tripPlanRequest);
        return tripPlanResponseDto;
    }
}
