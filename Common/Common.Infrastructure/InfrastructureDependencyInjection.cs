using Common.Application.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rebus.Transport.InMem;

namespace Common.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();
        return services;
    }
}
