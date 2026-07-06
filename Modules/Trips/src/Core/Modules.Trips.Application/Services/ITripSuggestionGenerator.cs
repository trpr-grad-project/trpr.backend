using Common.Application.Exceptions;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public interface ITripSuggestionGenerator
    {
        public Task<Dictionary<int, RankedItinerary>> GenerateTrip(DateTime startDate, int numberOfDays, Theme theme, ICollection<PlaceDto> places);
    }

    public class AiTripSuggestionGenerator(
    ) : ITripSuggestionGenerator
    {
        public async Task<Dictionary<int, RankedItinerary>> GenerateTrip(DateTime startDate, int numberOfDays, Theme theme, ICollection<PlaceDto> places)
        {
            throw new NotImplementedException("Not implemented yet");
        }
    }

    public class AlgorithmicSuggestionGenerator(IThemeProvider themeProvider, IItineraryRecommendationEngine itineraryRecommendationEngine) : ITripSuggestionGenerator
    {
        public async Task<Dictionary<int, RankedItinerary>> GenerateTrip(DateTime startDate, int numberOfDays, Theme theme, ICollection<PlaceDto> places)
        {
            var themeDefinition = themeProvider
                .Get(theme.Id);
            var categoryLimits = themeDefinition
                .Generator
                .CategoryLimits
                .ToDictionary(x =>
                    x.Category,
                    x => x.MaxPlaces);

            Dictionary<int, RankedItinerary> rankedItineraries = new();
            for (int numberOfDay = 1; numberOfDay <= numberOfDays; numberOfDay++)
            {
                var placess = new TripPlanGenerator()
                .GeneratePlans([.. places], categoryLimits, 11);
                RankedItinerary rankedItinerary = itineraryRecommendationEngine
                    .Rank(placess, themeDefinition)
                    .FirstOrDefault() ?? throw new NotFoundException("Trip.NotFound");
                rankedItineraries[numberOfDay] = rankedItinerary;
                var placesToRemoveIds = rankedItinerary.Itinerary.Places.Select(x => x.Id).ToList();
                var placesToRemove = places.Where(x => placesToRemoveIds.Contains(x.Id)).ToList();
                foreach (var place in placesToRemove)
                {
                    places.Remove(place);
                }
            }
            return rankedItineraries;
        }
    }
}