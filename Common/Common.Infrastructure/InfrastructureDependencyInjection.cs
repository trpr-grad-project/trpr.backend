using Common.Application.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace Common.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRebus(configure => configure
            .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), "my-queue"))
            .Options(o => o.SetNumberOfWorkers(1))
        );
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();
        return services;
    }
}
