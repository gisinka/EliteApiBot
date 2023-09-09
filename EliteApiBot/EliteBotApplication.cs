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
        private IServiceProvider serviceProvider;

        public Task InitializeAsync(IVostokHostingEnvironment environment)
        {
            serviceProvider = ConfigureServiceProvider(environment.ConfigurationProvider, environment.Log);
            return Task.CompletedTask;
        }

        public async Task RunAsync(IVostokHostingEnvironment environment)
        {
            await serviceProvider.GetRequiredService<CommandHandler>()
                .RunAsync(environment.ConfigurationProvider.Get<BotConfiguration>().DiscordToken, CancellationToken.None);
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
