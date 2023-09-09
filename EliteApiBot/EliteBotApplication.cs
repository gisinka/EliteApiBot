using Discord.Commands;
using Discord.WebSocket;
using EliteApiBot.Infrastructure.Discord;
using EliteApiBot.Infrastructure.Squad;
using Vostok.Hosting.Abstractions;
using Vostok.Logging.Abstractions;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace EliteApiBot
{
    public class EliteBotApplication : IVostokApplication
    {
        private CommandHandler commandHandler;

        public async Task InitializeAsync(IVostokHostingEnvironment environment)
        {
            var serviceProvider = ConfigureServiceProvider(environment.ConfigurationProvider, environment.Log);
            commandHandler = serviceProvider.GetRequiredService<CommandHandler>();
        }

        public async Task RunAsync(IVostokHostingEnvironment environment)
        {
            await commandHandler.RunAsync(environment.ConfigurationProvider.Get<BotConfiguration>().DiscordToken, environment.ShutdownToken);
        }

        private static ServiceProvider ConfigureServiceProvider(IConfigurationProvider configurationProvider, ILog log)
        {
            var botConfiguration = configurationProvider.Get<BotConfiguration>();
            return new ServiceCollection()
                .AddSingleton(configurationProvider)
                .AddSingleton<IEliteApiClient, EliteApiClient>()
                .AddSingleton(new DiscordSocketConfig { LogLevel = botConfiguration.LogSeverity })
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(new CommandServiceConfig { LogLevel = botConfiguration.LogSeverity, CaseSensitiveCommands = false })
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton(log)
                .BuildServiceProvider();
        }
    }
}
