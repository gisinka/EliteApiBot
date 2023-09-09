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
        public string ApiUrl { get; set; } = "https://sapi.demb.uk/api";
        public string CsvLink { get; set; } = "https://gitea.demb.uk/a31/CEC-list-monitoring/raw/branch/master/list.csv";
        public string JsonLinkWithTagsResolve { get; set; } = "squads/now/by-tag/extended/{0}?resolve_tags=true&pretty_keys=true";
        public string JsonLinkWithoutTagsResolve { get; set; } = "squads/now/by-tag/extended/{0}?pretty_keys=true";

    }
}
