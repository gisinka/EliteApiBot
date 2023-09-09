using Discord;
using Vostok.Configuration.Sources.Object;
using Vostok.Hosting;
using Vostok.Hosting.Setup;

namespace EliteApiBot;

public static class Program
{
    public static async Task Main()
    {
        var host = new VostokHost(new VostokHostSettings(new EliteBotApplication(), builder =>
        {
            builder.SetupApplicationIdentity(setup => setup
                .SetApplication("EliteApiBot")
                .SetEnvironment("default")
                .SetInstance("single")
                .SetProject("Isin")
                .SetSubproject("Elite"));
            builder.SetupLog(setup => setup.SetupConsoleLog(consoleLogSetup => consoleLogSetup.Enable().UseAsynchronous()));
            builder.SetupConfiguration(setup =>
            {
                setup.SetupSourceFor<BotConfiguration>()
                    .AddSource(new ObjectSource(new BotConfiguration 
                    { 
                        LogSeverity = LogSeverity.Info, 
                        DiscordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN")!,
                    }));
            });
        }));

        await host.RunAsync();
    }
}