using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Elite_API_Discord.Infrastructure.Discord;

namespace Elite_API_Discord;

public class Program
{
    public static async Task Main()
    {
        var client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info });
        var commandsService = new CommandService(new CommandServiceConfig { LogLevel = LogSeverity.Info, CaseSensitiveCommands = false });
        var serviceProvider = Initialize.BuildServiceProvider(client, commandsService);
        await commandsService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
        await new CommandHandler(commandsService, client)
            .RunAsync("OTIwMjUyMTYxMTE4NTY4NDY5.YbhpnA.nCd16zUnCeS5hY7odT_cTS8hkzk");
    }
}