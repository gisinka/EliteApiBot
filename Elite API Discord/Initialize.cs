using Discord.Commands;
using Discord.WebSocket;
using Elite_API_Discord.Infrastructure.Discord;

namespace Elite_API_Discord;

public class Initialize
{
    private readonly DiscordSocketClient client;
    private readonly CommandService commands;

    // Ask if there are existing CommandService and DiscordSocketClient
    // instance. If there are, we retrieve them and add them to the
    // DI container; if not, we create our own.
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