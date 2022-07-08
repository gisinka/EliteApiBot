using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Elite_API_Discord.Infrastructure.Discord;

namespace Elite_API_Discord;

public class Program
{
    public static async Task Main()
    {
        var serviceProvider = new Initialize().BuildServiceProvider();
        var commandsService = serviceProvider.GetService<CommandService>();
        await commandsService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
        await new CommandHandler(commandsService, serviceProvider.GetService<DiscordSocketClient>())
            .RunAsync("OTIwMjUyMTYxMTE4NTY4NDY5.YbhpnA.nCd16zUnCeS5hY7odT_cTS8hkzk");
    }
}