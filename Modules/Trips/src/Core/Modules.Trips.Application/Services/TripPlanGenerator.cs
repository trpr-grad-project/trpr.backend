using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Application.Services
{
    public interface IItineraryRecommendationEngine
    {
        IReadOnlyList<RankedItinerary> Rank(
       IReadOnlyList<Itinerary> itineraries,
       ThemeDefinition theme);
    }

    public interface IThemeScorer
    {
        ItineraryScore Score(
            Itinerary itinerary,
            ThemeDefinition theme);
    }
    public sealed class ItineraryScore
    {
        public double ThemeScore { get; init; }

        public double RatingScore { get; init; }

        public double CategoryScore { get; init; }

        public double TravelPenalty { get; init; }

        public double TotalScore { get; init; }
    }
    public sealed class Itinerary
    {
        public IReadOnlyList<PlaceDto> Places { get; init; } = [];
    }
    public sealed class RankedItinerary
    {
        public Itinerary Itinerary { get; init; } = default!;

        public ItineraryScore Score { get; init; } = default!;
    }
    public class TripPlanGenerator
    {
        private List<PlaceDto> _candidates = [];
        private Dictionary<string, int> _categoryLimits = [];
        private double _totalTime;
        private List<List<PlaceDto>> _results = [];

        public List<Itinerary> GeneratePlans(
            List<PlaceDto> candidates,
            Dictionary<string, int> categoryLimits,
            double totalTime)
        {
            _candidates = candidates;
            _categoryLimits = categoryLimits;
            _totalTime = totalTime;
            _results = [];

            Backtrack(
                startIndex: 0,
                currentPlan: [],
                currentTime: 0,
                categoryUsage: []);

            return [.. _results.Select(plan => new Itinerary { Places = plan })];
        }

        private void Backtrack(
            int startIndex,
            List<PlaceDto> currentPlan,
            double currentTime,
            Dictionary<string, int> categoryUsage)
        {
            if (currentPlan.Count > 0)
            {
                _results.Add([.. currentPlan]);
            }

            for (int i = startIndex; i < _candidates.Count; i++)
            {
                var place = _candidates[i];
                var visitTime = place.AverageVisitTime ?? 1;

                double travelTime = 0;

                if (currentPlan.Count > 0)
                {
                    var lastPlace = currentPlan.Last();

                    travelTime = GeoUtils.DistanceInMeters(
                        lastPlace.Latitude,
                        lastPlace.Longitude,
                        place.Latitude,
                        place.Longitude) / 1000 * 60;
                }

                if (!CanAddPlace(place, visitTime + travelTime, currentTime, categoryUsage))
                    continue;

                AddPlace(place, visitTime + travelTime, currentPlan, categoryUsage, ref currentTime);

                Backtrack(i + 1, currentPlan, currentTime, categoryUsage);

                RemovePlace(place, visitTime + travelTime, currentPlan, categoryUsage, ref currentTime);
            }
        }

        private bool CanAddPlace(
            PlaceDto place,
            double visitTime,
            double currentTime,
            Dictionary<string, int> categoryUsage)
        {
            if (currentTime + visitTime > _totalTime)
                return false;

            if (_categoryLimits.TryGetValue(place.Category.Name, out int maxLimit))
            {
                if (categoryUsage.GetValueOrDefault(place.Category.Name) >= maxLimit)
                    return false;
            }

            return true;
        }

        private static void AddPlace(
            PlaceDto place,
            double visitTime,
            List<PlaceDto> currentPlan,
            Dictionary<string, int> categoryUsage,
            ref double currentTime)
        {
            currentPlan.Add(place);
            currentTime += visitTime;

            categoryUsage[place.Category.Name] =
                categoryUsage.GetValueOrDefault(place.Category.Name) + 1;
        }

        private static void RemovePlace(
            PlaceDto place,
            double visitTime,
            List<PlaceDto> currentPlan,
            Dictionary<string, int> categoryUsage,
            ref double currentTime)
        {
            currentPlan.RemoveAt(currentPlan.Count - 1);
            currentTime -= visitTime;
            categoryUsage[place.Category.Name]--;
        }
    }
}