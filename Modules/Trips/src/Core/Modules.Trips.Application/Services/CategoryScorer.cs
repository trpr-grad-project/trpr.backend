using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Application.Services
{
    public class CategoryScorer : ICategoryScorer
    {
        public double Calculate(Itinerary itinerary, ThemeDefinition theme)
        {
            var weights =
                theme.Scoring.Categories.ToDictionary(
                    x => x.Category,
                    x => x.Weight);

            return itinerary.Places.Sum(place =>
                weights.GetValueOrDefault(
                    place.Category.Name,
                    0));
        }
    }
}