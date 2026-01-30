using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Persistence.Outbox;
using Modules.Notifications.Persistence.Repositories;
using Modules.Notifications.Persistence.Data;
using Modules.Notifications.Persistence.Inbox;

namespace Modules.Notifications.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("RommieDb")!;
        services.AddDbContext<AppDbContext>((sp, options) =>
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
        services.AddScoped<IBoxMessageManager, BoxMessageManager>();
        services.Configure<OutBoxOptions>(configuration.GetSection("Notifications:OutBox"));
        services.Configure<InBoxOptions>(configuration.GetSection("Notifications:Inbox"));
        services.AddScoped<IDbConnectionFactory>(x => new DbConnectionFactory(dbConnectionString));
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<AppDbContext>());
        // adding quartz for background jobs 
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        services.ConfigureOptions<ConfigureProcessInboxJob>();
        return services;
    }
}
