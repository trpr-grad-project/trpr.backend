using Microsoft.Extensions.DependencyInjection;
using Modules.Trips.Application.Services;

namespace Modules.Trips.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        #endregion

        #region  factories
        #endregion

        #region  services
        services.AddScoped<PlaceService>();
        services.AddScoped<TripService>();
        services.AddScoped<BiddingService>();
        services.AddScoped<ReviewService>();
        services.AddScoped<ITripSuggestionGenerator, AlgorithmicSuggestionGenerator>();
        services.AddScoped<ICategoryScorer, CategoryScorer>();
        services.AddScoped<IThemeProvider, ThemeProvider>();
        services.AddScoped<ITagScorer, TagScorer>();
        services.AddScoped<IRatingScorer, RatingScorer>();
        services.AddScoped<IThemeScorer, ThemeScorer>();
        services.AddScoped<ITravelPenaltyCalculator, TravelPenaltyCalculator>();
        services.AddTransient<TripPlanGenerator>();
        services.AddScoped<IItineraryRecommendationEngine, ItineraryRecommendationEngine>();
        #endregion

        return services;
    }


}
