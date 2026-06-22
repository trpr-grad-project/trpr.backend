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
        services.AddScoped<ITripSuggestionGenerator, AiTripSuggestionGenerator>();
        #endregion

        return services;
    }


}
