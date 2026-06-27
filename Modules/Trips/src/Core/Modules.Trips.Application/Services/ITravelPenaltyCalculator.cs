namespace Modules.Trips.Application.Services
{
    public interface ITravelPenaltyCalculator
    {
        double Calculate(Itinerary itinerary);
    }
}