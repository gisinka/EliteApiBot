using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EliteApiBot.Extensions;
using Vostok.Logging.Abstractions;
using IResult = Discord.Commands.IResult;

namespace EliteApiBot.Infrastructure.Discord;

public class CommandHandler
{
    private readonly DiscordSocketClient client;
    private readonly CommandService commandsService;
    private readonly IServiceProvider services;
    private readonly ILog log;

    public CommandHandler(CommandService commandsService, DiscordSocketClient client, IServiceProvider services, ILog log)
    {
        this.commandsService = commandsService;
        this.client = client;
        this.services = services;
        this.log = log;
    }

    public async Task RunAsync(string token, CancellationToken cancellationToken)
    {
        await InitializeAsync();
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        await Task.Delay(Timeout.Infinite, CancellationToken.None);
    }

    private Task Log(LogMessage logMessage)
    {
        log.Log(logMessage.Convert());
        return Task.CompletedTask;
    }

    private async Task InitializeAsync()
    {
        await commandsService.AddModulesAsync(Assembly.GetEntryAssembly(), services);
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

            await commandsService.ExecuteAsync(context, argPos, services);
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

            logMessage = new LogMessage(LogSeverity.Error, "Command", $"Sorry, {context.User.Username}. something went wrong -> [{result}]!");
            Console.WriteLine(logMessage.ToString());
        });

        return Task.CompletedTask;
    }
}