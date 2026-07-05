using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Modules.Payments.Application.Abstractions;
using Quartz;
using Modules.Payments.Infrastructure.Data;
using Modules.Payments.Infrastructure.Repositories;
using Modules.Payments.Infrastructure.Inbox;
using Modules.Payments.Infrastructure.Outbox;
using Modules.Payments.Contracts.Contracts;

namespace Modules.Payments.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("RommieDb")!;
        services.AddDbContext<PaymentsDbContext>((sp, options) =>
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
        services.Configure<OutBoxOptions>(configuration.GetSection("Payments:OutBox"));
        services.Configure<InBoxOptions>(configuration.GetSection("Payments:Inbox"));
        services.AddScoped<IDbConnectionFactory>(x => new DbConnectionFactory(dbConnectionString));
        services.AddScoped<RepositoryFactory>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IPaymentsDbContext>(x => x.GetRequiredService<PaymentsDbContext>());
        services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<PaymentsDbContext>());
        // adding quartz for background jobs 
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        services.ConfigureOptions<ConfigureProcessInboxJob>();
        services.AddScoped<IPayContract, PayContract>();
        return services;
    }
}
