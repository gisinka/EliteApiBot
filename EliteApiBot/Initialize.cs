using Discord.Commands;
using Discord.WebSocket;

namespace Elite_API_Discord
{
    public static class Initialize
    {
        public static IServiceProvider BuildServiceProvider(DiscordSocketClient client, CommandService commandsService) => new ServiceCollection()
            .AddSingleton(client)
            .AddSingleton(commandsService)
            .BuildServiceProvider();
    }
}
