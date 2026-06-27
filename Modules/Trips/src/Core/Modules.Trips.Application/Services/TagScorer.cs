using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public class TagScorer : ITagScorer
    {
        public double Calculate(PlaceDto place, Dictionary<string, double> tagWeights, double decayFactor = 1)
        {
            double total = 0;
            var weights = place.Tags
                                .Select(x =>
                                    tagWeights.GetValueOrDefault(
                                        x.Name,
                                        0))
                                .Where(x => x != 0)
                                .OrderByDescending(x => x)
                                .ToList();

            double multiplier = 1;

            foreach (var weight in weights)
            {
                total += weight * multiplier;

                multiplier *=
                    decayFactor;
            }
            return total;
        }
    }
}