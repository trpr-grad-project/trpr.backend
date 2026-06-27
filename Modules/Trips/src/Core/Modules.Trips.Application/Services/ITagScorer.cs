using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public interface ITagScorer
    {
        double Calculate(PlaceDto place, Dictionary<string, double> tagWeights, double decayFactor = 1);
    }
}