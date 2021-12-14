using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Elite_API_Discord.Infrastructure.Discord
{
    internal class MainTask
    {
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        public async Task MainAsync(string token)
        {
            client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info, });

            client.Log += Log;

            commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
            });
            client.Log += Log;
            commands.Log += Log;
            services = new Initialize(commands, client).BuildServiceProvider();
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

        public async Task InitializeAsync()
        {
            await commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: services);
            client.MessageReceived += HandleCommandAsync;
        }

        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (messageParam is not SocketUserMessage message) return;
            var argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;
            var context = new SocketCommandContext(client, message);
            await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }
}
