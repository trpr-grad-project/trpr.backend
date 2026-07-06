namespace Modules.Trips.Application.Services;

using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

public interface IAiTripSuggestionService
{
    Task<List<List<SuggestedPlaceDto>>> GenerateTripPlan(DateTime startDate, Theme theme, ICollection<PlaceDto> places, int numberOfDays);
}
