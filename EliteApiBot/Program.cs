using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EliteApiBot.Infrastructure.Discord;
using EliteApiBot.Infrastructure.Squad;
using EliteApiBot.Utils;

namespace EliteApiBot;

public static class Program
{
    public static async Task Main()
    {
        var serviceProvider = ConfigureServiceProvider();

        await serviceProvider.GetRequiredService<CommandHandler>()
            .RunAsync("OTIwMjUyMTYxMTE4NTY4NDY5.YbhpnA.nCd16zUnCeS5hY7odT_cTS8hkzk");
    }

    private static ServiceProvider ConfigureServiceProvider()
    {
        return new ServiceCollection()
            .AddSingleton(new HttpClient().AddHeaders())
            .AddSingleton<SquadRequester>()
            .AddSingleton<SquadBuilder>()
            .AddSingleton(new DiscordSocketConfig { LogLevel = LogSeverity.Info })
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(new CommandServiceConfig { LogLevel = LogSeverity.Info, CaseSensitiveCommands = false })
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandler>()
            .BuildServiceProvider();
    }
}