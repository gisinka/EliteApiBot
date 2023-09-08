using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EliteApiBot.Infrastructure.Discord;
using EliteApiBot.Infrastructure.Squad;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;

namespace EliteApiBot;

public static class Program
{
    public static async Task Main()
    {
        var serviceProvider = ConfigureServiceProvider();

        await serviceProvider.GetRequiredService<CommandHandler>()
            .RunAsync(Environment.GetEnvironmentVariable("DISCORD_TOKEN")!, CancellationToken.None);
    }

    private static ServiceProvider ConfigureServiceProvider()
    {
        return new ServiceCollection()
            .AddSingleton<IEliteApiClient, EliteApiClient>()
            .AddSingleton<SquadBuilder>()
            .AddSingleton(new DiscordSocketConfig { LogLevel = LogSeverity.Info })
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(new CommandServiceConfig { LogLevel = LogSeverity.Info, CaseSensitiveCommands = false })
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandler>()
            .AddSingleton<ILog>(new ConsoleLog())
            .BuildServiceProvider();
    }
}