using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.Users.Application.Interfaces;
using Modules.Users.Infrastructure.Services;
using Modules.Users.Infrastructure.Options;
using Modules.Users.Infrastructure.Delegates;
using Modules.Users.Infrastructure.Clients;
using Modules.Users.Infrastructure.Data;
using Modules.Users.Infrastructure.Outbox;
using Modules.Users.Infrastructure.Inbox;
using Modules.Users.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Repositories;
using Modules.Users.Infrastructure.Repositories;
using Quartz;

namespace Modules.Users.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));
        services.AddTransient<AdminKeyCloakAuthDelegatingHandler>();
        services.AddHttpClient<AdminKeyCloakClient>((sp, client) =>
        {
            KeyCloakOptions options = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            client.BaseAddress = new Uri(options.AdminUrl);
        })
        .AddHttpMessageHandler<AdminKeyCloakAuthDelegatingHandler>();
        services.AddHttpClient<TokenKeyCloackCLient>((sp, client) =>
        {
            KeyCloakOptions options = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            client.BaseAddress = new Uri(options.TokenUrl);
        });
        services.AddHttpClient<SemanticModelClient>((sp, client) =>
        {
            string baseUrl = configuration.GetConnectionString("Users:SemanticModelUrl")!;
            client.BaseAddress = new Uri(baseUrl);
        });
        services.AddScoped<IIdentityProviderService, IdentityProviderService>();

        string dbConnectionString = configuration.GetConnectionString("RommieDb")!;
        services.AddDbContext<UsersDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(dbConnectionString, op =>
                {
                    op.MigrationsAssembly(AssemblyRefrence.Assembly);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<PublishOutboxMessagesInterceptor>());
        });
        services.AddScoped<PublishOutboxMessagesInterceptor>();
        services.Configure<OutBoxOptions>(configuration.GetSection("Users:OutBox"));
        services.Configure<InBoxOptions>(configuration.GetSection("Users:InBox"));
        services.AddScoped<IDbConnectionFactory>(x => new DbConnectionFactory(dbConnectionString));
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUsersDbContext>(x => x.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<UsersDbContext>());
        // adding quartz for background jobs 
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        services.ConfigureOptions<ConfigureProcessInboxJob>();
        return services;
    }
}
