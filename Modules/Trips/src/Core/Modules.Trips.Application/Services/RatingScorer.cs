namespace Modules.Trips.Application.Services
{
    public class RatingScorer : IRatingScorer
    {
        public double Calculate(Itinerary itinerary)
        {
            if (itinerary.Places.Count == 0)
                return 0;

            return itinerary.Places
                .Average(x => (0) / 5.0 * 10);
        }
    }
}