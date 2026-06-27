using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Application.Services
{
    public class ThemeScorer : IThemeScorer
    {
        private readonly ITagScorer _tagScorer;
        private readonly IRatingScorer _ratingScorer;
        private readonly ICategoryScorer _categoryScorer;
        private readonly ITravelPenaltyCalculator _travelPenaltyCalculator;

        public ThemeScorer(
            ITagScorer tagScorer,
            IRatingScorer ratingScorer,
            ICategoryScorer categoryScorer,
            ITravelPenaltyCalculator travelPenaltyCalculator)
        {
            _tagScorer = tagScorer;
            _ratingScorer = ratingScorer;
            _categoryScorer = categoryScorer;
            _travelPenaltyCalculator = travelPenaltyCalculator;
        }

        public ItineraryScore Score(
            Itinerary itinerary,
            ThemeDefinition theme)
        {
            var tagWeights =
                theme.Scoring.Tags.ToDictionary(
                    x => x.Tag,
                    x => x.Weight);

            double themeScore =
                itinerary.Places.Sum(x =>
                    _tagScorer.Calculate(x, tagWeights, theme.Scoring.Strategy.TagDecayFactor));

            double ratingScore =
                _ratingScorer.Calculate(itinerary);

            double categoryScore =
                _categoryScorer.Calculate(itinerary, theme);

            double travelPenalty =
                _travelPenaltyCalculator.Calculate(itinerary);

            var strategy = theme.Scoring.Strategy;

            return new ItineraryScore
            {
                ThemeScore = themeScore,
                RatingScore = ratingScore,
                CategoryScore = categoryScore,
                TravelPenalty = travelPenalty,

                TotalScore =
                    themeScore * strategy.ThemeWeight
                    + ratingScore * strategy.RatingWeight
                    + categoryScore * strategy.CategoryWeight
                    - travelPenalty * strategy.TravelPenaltyWeight
            };
        }
    }
}