﻿using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using EliteApiBot.Infrastructure.Discord;
using EliteApiBot.Infrastructure.Squad;
using Vostok.Hosting.Abstractions;
using Vostok.Logging.Abstractions;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace EliteApiBot;

public class EliteBotApplication : IVostokApplication
{
    public async Task RunAsync(IVostokHostingEnvironment environment)
    {
        var serviceProvider = ConfigureServiceProvider(environment.ConfigurationProvider, environment.Log);
        var commandHandler = serviceProvider.GetRequiredService<CommandHandler>();
        await commandHandler.RunAsync(environment.ConfigurationProvider.Get<BotConfiguration>().DiscordToken, environment.ShutdownToken);
    }

    public Task InitializeAsync(IVostokHostingEnvironment environment)
    {
        return Task.CompletedTask;
    }

    private static ServiceProvider ConfigureServiceProvider(IConfigurationProvider configurationProvider, ILog log)
    {
        var botConfiguration = configurationProvider.Get<BotConfiguration>();
        return new ServiceCollection()
            .AddSingleton(configurationProvider)
            .AddSingleton<IEliteApiClient, EliteApiClient>()
            .AddSingleton(new DiscordSocketConfig
            {
                LogLevel = botConfiguration.LogSeverity,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent

            })
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(new CommandServiceConfig { LogLevel = botConfiguration.LogSeverity, CaseSensitiveCommands = false })
            .AddSingleton<CommandService>()
            .AddSingleton<InteractionService>()
            .AddSingleton<CommandHandler>()
            .AddSingleton(log)
            .BuildServiceProvider();
    }
}