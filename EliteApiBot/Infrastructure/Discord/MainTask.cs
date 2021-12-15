using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Elite_API_Discord.Infrastructure.Discord;

internal class MainTask
{
    private readonly DiscordSocketClient client;
    private readonly CommandService commands;
    private readonly IServiceProvider services;

    public MainTask()
    {
        client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info });
        commands = new CommandService(new CommandServiceConfig { LogLevel = LogSeverity.Info, CaseSensitiveCommands = false });
        client.Log += Log;
        commands.Log += Log;
        services = new Initialize(commands, client).BuildServiceProvider();
    }

    public async Task MainAsync(string token)
    {
        await InitializeAsync();
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task InitializeAsync()
    {
        await commands.AddModulesAsync(
            Assembly.GetEntryAssembly(),
            services);

        client.MessageReceived += HandleCommandAsync;
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        if (messageParam is not SocketUserMessage message) return;

        var argPos = 0;

        if (!(message.HasCharPrefix('!', ref argPos) ||
              message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
            return;

        var context = new SocketCommandContext(client, message);

        await commands.ExecuteAsync(
            context,
            argPos,
            null);
    }
}