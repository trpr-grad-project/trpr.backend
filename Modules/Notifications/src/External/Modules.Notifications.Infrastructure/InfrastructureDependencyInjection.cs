using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Notifications.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // if (configuration.GetValue<bool>("UseDummyEmbeddingService"))
        //     services.AddScoped<IEmbeddingService, EmbeddingDummyService>();
        // else
        //     services.AddScoped<IEmbeddingService, EmbeddingService>();
        // services.AddScoped<IClaimsTransformation, KeyCloackClaimsTransformation>();
        // services.AddAuthenticationInternal();
        return services;
    }
}
