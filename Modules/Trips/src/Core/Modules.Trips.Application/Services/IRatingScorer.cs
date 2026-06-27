namespace Modules.Trips.Application.Services
{
    public interface IRatingScorer
    {
        double Calculate(Itinerary itinerary);
    }
}