using Common.Application.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rebus.Config;
using Rebus.Transport.InMem;

namespace Common.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRebus(cfg => cfg
            .Logging(l => l.Console())
            .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), "Modular-Monolith-Queue")));
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();
        return services;
    }
}
