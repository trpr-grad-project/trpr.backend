using Microsoft.Extensions.DependencyInjection;
using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents.Extensions;

namespace Modules.Notifications.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region Options
        #endregion

        #region  factories
        #endregion

        #region  services
        services.AddScoped<UserService>();
        services.AddScoped<NotificationService>();
        #endregion
        return services;
    }

}
