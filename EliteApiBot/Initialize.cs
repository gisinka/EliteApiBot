using Discord.Commands;
using Discord.WebSocket;

namespace Elite_API_Discord;

public class Initialize
{
    private readonly DiscordSocketClient client;
    private readonly CommandService commands;

    public Initialize(CommandService commands = null, DiscordSocketClient client = null)
    {
        this.commands = commands ?? new CommandService();
        this.client = client ?? new DiscordSocketClient();
    }

    public IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddSingleton(client)
            .AddSingleton(commands)
            .BuildServiceProvider();
    }
}