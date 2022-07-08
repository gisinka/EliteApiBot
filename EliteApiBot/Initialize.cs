using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Elite_API_Discord
{
    public class Initialize
    {
        private readonly CommandService commandsService;
        private readonly DiscordSocketClient client;

        public Initialize(CommandService? commandsService = null, DiscordSocketClient? client = null)
        {
            this.commandsService = commandsService ?? new CommandService(new CommandServiceConfig { LogLevel = LogSeverity.Info, CaseSensitiveCommands = false });
            this.client = client ?? new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info });
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(commandsService)
            .AddSingleton(client)
            .BuildServiceProvider();
    }
}
