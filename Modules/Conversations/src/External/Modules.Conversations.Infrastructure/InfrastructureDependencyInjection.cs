using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Modules.Conversations.Infrastructure.Repositories;
using Modules.Conversations.Infrastructure.Data;
using Modules.Conversations.Infrastructure.Inbox;
using Modules.Conversations.Infrastructure.Outbox;
using Modules.Conversations.Application.Abstractions;
using Microsoft.Extensions.AI;
using Modules.Conversations.Infrastructure.Services;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Contracts.Contracts;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;
using Modules.Conversations.Infrastructure.Delegates;

namespace Modules.Conversations.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IChatClient>(sp =>
        {
            IChatClient baseClient =
                new ChatClient(
                    model: configuration["Conversations:OpenRouter:Model"]!,
                    new ApiKeyCredential(configuration["Conversations:OpenRouter:ApiKey"]!),
                    new OpenAIClientOptions
                    {
                        Endpoint = new Uri(configuration["Conversations:OpenRouter:baseUrl"]!),
                    }
                )
                .AsIChatClient();

            return new ChatClientBuilder(baseClient)
                .UseFunctionInvocation()
                .Build();
        });

        string dbConnectionString = configuration.GetConnectionString("RommieDb")!;
        services.AddDbContext<ConversationsDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(dbConnectionString, op =>
                {
                    op.MigrationsAssembly(AssemblyRefrence.Assembly);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<PublishOutboxMessagesInterceptor>());
        });
        services.AddScoped<INotificationSender, NotificationSender>();
        services.AddScoped<PublishOutboxMessagesInterceptor>();
        services.Configure<OutBoxOptions>(configuration.GetSection("Conversations:OutBox"));
        services.Configure<InBoxOptions>(configuration.GetSection("Conversations:InBox"));
        services.AddScoped<RepositoryFactory>();
        services.AddScoped<IAiChatService, AiChatService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<ConversationsDbContext>());
        services.AddScoped<IDbConnectionFactory>(x => new DbConnectionFactory(dbConnectionString));
        services.AddScoped<IConversationsDbContext>(x => x.GetRequiredService<ConversationsDbContext>());
        // adding quartz for background jobs 
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        services.ConfigureOptions<ConfigureProcessInboxJob>();
        services.AddScoped<IConversationsContract, ConversationContract>();
        return services;
    }
}
