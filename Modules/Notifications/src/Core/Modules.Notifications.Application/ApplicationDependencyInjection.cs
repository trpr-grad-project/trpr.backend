using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application.Pipelines;
using Common.Application.DomainEvents.Extensions;

namespace Modules.Notifications.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        #endregion

        #region  factories
        #endregion

        #region  services
        #endregion
        services.AddDomainEventHandlers(cfg =>
            cfg
                .AddAssembly(Application.AssemblyRefrence.Assembly)
        );
        services.AddDomainEventHandlerDecorators(cfg =>
            cfg
                .AddPipeline(typeof(OutboxIdempotentDomainEventHandlerDecorator<>))
                .AddAssembly(Application.AssemblyRefrence.Assembly));
        return services;
    }

}
