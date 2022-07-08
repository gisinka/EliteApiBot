using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IResult = Discord.Commands.IResult;

namespace Elite_API_Discord.Infrastructure.Discord;

internal class CommandHandler
{
    private readonly DiscordSocketClient client;
    private readonly CommandService commandsService;

    public CommandHandler(CommandService commandsService, DiscordSocketClient client)
    {
        this.commandsService = commandsService;
        this.client = client;
    }

    public async Task RunAsync(string token)
    {
        InitializeAsync();
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private void InitializeAsync()
    {
        client.Log += Log;
        commandsService.Log += Log;
        client.MessageReceived += HandleCommand;
        commandsService.CommandExecuted += CommandServiceExecutedLog;
    }

    private Task HandleCommand(SocketMessage messageParam)
    {
        Task.Run(async () =>
        {
            if (messageParam is not SocketUserMessage message) return;

            var argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(client, message);

            await commandsService.ExecuteAsync(context, argPos, null);
        });

        return Task.CompletedTask;
    }

    private static Task CommandServiceExecutedLog(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        Task.Run(() =>
        {
            LogMessage logMessage;

            if (!command.IsSpecified)
            {
                logMessage = new LogMessage(LogSeverity.Error, "Command", $"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                Console.WriteLine(logMessage.ToString());
                return;
            }

            if (result.IsSuccess)
            {
                logMessage = new LogMessage(LogSeverity.Info, "Command", $"Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
                Console.WriteLine(logMessage.ToString());
                return;
            }

            logMessage = new LogMessage(LogSeverity.Error, "Command", $"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
            Console.WriteLine(logMessage.ToString());
        });

        return Task.CompletedTask;
    }
}