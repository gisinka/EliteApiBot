using Discord;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure;

public static class Constants
{
    public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
    public const string ShortLink = "https://sapi.demb.design/squads/now/by-tag/short/{0}";
    public const string ExtendedLink = "https://sapi.demb.design/squads/now/by-tag/extended/{0}?resolve_tags=true";
    public const string CsvLink = "https://gitea.demb.design/a31/CEC-list-monitoring/raw/branch/master/list.csv";

    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    public static readonly Embed InvalidTagEmbed = new EmbedBuilder
        {
            Fields = new List<EmbedFieldBuilder>
            {
                new() { IsInline = false, Name = "Status", Value = "Tag is invalid" }
            }
        }
        .WithColor(Color.Gold)
        .Build();

    public static readonly Embed NotExistingTagEmbed = new EmbedBuilder
        {
            Fields = new List<EmbedFieldBuilder>
            {
                new() { IsInline = false, Name = "Status", Value = "Tag does not exist" }
            }
        }
        .WithColor(Color.Gold)
        .Build();

    public static readonly Embed NotExistingNameEmbed = new EmbedBuilder
        {
            Fields = new List<EmbedFieldBuilder>
            {
                new() { IsInline = false, Name = "Status", Value = "Name does not exist" }
            }
        }
        .WithColor(Color.Gold)
        .Build();

    public static readonly Embed ApiFaultEmbed = new EmbedBuilder
        {
            Fields = new List<EmbedFieldBuilder>
            {
                new() { IsInline = false, Name = "Status", Value = "Небольшие технические шоколадки, упало апи, alert" }
            }
        }
        .WithColor(Color.DarkRed)
        .Build();
}