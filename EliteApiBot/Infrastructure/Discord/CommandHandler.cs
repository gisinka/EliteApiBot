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
        await Task.Delay(Timeout.Infinite, cancellationToken)
            .ContinueWith(_ => client.StopAsync())
            .ContinueWith(_ => client.LogoutAsync())
            .ContinueWith(_ => client.DisposeAsync());
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

    private async Task HandleCommand(SocketMessage messageParam)
     {
         if (messageParam is not SocketUserMessage message) 
             return;

         var argPos = 0;

         if (!(message.HasCharPrefix('!', ref argPos) ||
               message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
             message.Author.IsBot)
             return;

         var context = new SocketCommandContext(client, message);

         await commandsService.ExecuteAsync(context, argPos, services);
    }

    private Task CommandServiceExecutedLog(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        if (!command.IsSpecified)
        {
            log.Error($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
            return Task.CompletedTask;
        }

        if (result.IsSuccess)
        {
            log.Info($"Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
            return Task.CompletedTask;
        }

        log.Error($"Sorry, {context.User.Username}. something went wrong -> [{result}]!");

        return Task.CompletedTask;
    }
}