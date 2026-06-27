using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Application.Services
{
    public interface ICategoryScorer
    {
        double Calculate(Itinerary itinerary, ThemeDefinition theme);
    }
}