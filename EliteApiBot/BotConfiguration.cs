using Discord;
using Vostok.Configuration.Abstractions.Attributes;

namespace EliteApiBot
{
    public class BotConfiguration
    {
        [Secret]
        public string DiscordToken { get; set; }
        public LogSeverity LogSeverity { get; set; }
        public long PlayersCacheSizeLimit { get; set; } = 10 * 1024 * 1024;
        public TimeSpan PlayersCacheExpirationTimeout { get; set; } = TimeSpan.FromMinutes(1);

    }
}
