using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Application.Services
{
    public class ItineraryRecommendationEngine : IItineraryRecommendationEngine
    {
        private readonly IThemeProvider _themeProvider;
        private readonly IThemeScorer _themeScorer;

        public ItineraryRecommendationEngine(
            IThemeProvider themeProvider,
            IThemeScorer themeScorer)
        {
            _themeProvider = themeProvider;
            _themeScorer = themeScorer;
        }

        public IReadOnlyList<RankedItinerary> Rank(
            IReadOnlyList<Itinerary> itineraries,
            ThemeDefinition theme)
        {

            return itineraries
            .Select(x =>
            {
                var score = _themeScorer.Score(x, theme);

                return new RankedItinerary
                {
                    Itinerary = x,
                    Score = score
                };
            })
            .OrderByDescending(x => x.Score.TotalScore)
            .ToList();
        }
    }
}