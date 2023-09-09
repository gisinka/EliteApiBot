using Discord;
using Vostok.Configuration.Abstractions.Attributes;

namespace EliteApiBot
{
    public class BotConfiguration
    {
        [Secret]
        public string DiscordToken { get; set; }
        public LogSeverity LogSeverity { get; set; }
    }
}
