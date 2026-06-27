namespace Modules.Trips.Application.Services
{
    public class TravelPenaltyCalculator : ITravelPenaltyCalculator
    {
        public double Calculate(Itinerary itinerary)
        {
            if (itinerary.Places.Count <= 1)
                return 0;

            double totalMinutes = 0;

            for (int i = 1; i < itinerary.Places.Count; i++)
            {
                var from = itinerary.Places[i - 1];
                var to = itinerary.Places[i];

                var distance =
                    GeoUtils.DistanceInMeters(
                        from.Latitude,
                        from.Longitude,
                        to.Latitude,
                        to.Longitude);

                totalMinutes += distance / 1000.0 / 50.0 * 60.0;
            }

            return totalMinutes;
        }
    }
}