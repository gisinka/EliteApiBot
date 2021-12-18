using System.Globalization;
using Discord;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadInfoFull
{
    [JsonProperty("Squadron name")] public string SquadronName { get; set; }

    [JsonProperty("Tag")] public string Tag { get; set; }

    [JsonProperty("Members")] public short Members { get; set; }

    [JsonProperty("Owner")] public string Owner { get; set; }

    [JsonProperty("Platform")] public string Platform { get; set; }

    [JsonProperty("Created UTC")] public DateTime CreationDate { get; set; }

    [JsonProperty("Power name")] public string Power { get; set; } = "N/D";

    [JsonProperty("Super power name")] public string SuperPower { get; set; } = "N/D";

    [JsonProperty("Faction name")] public string Faction { get; set; } = "N/D";

    [JsonProperty("User tags")] public string UserTags { get; set; }

    [JsonProperty("Updated UTC")] public DateTime UpdatedDate { get; set; }

    [JsonProperty("squad_id")] public long SquadId { get; set; }

    public Embed GetEmbed()
    {
        var builder = new EmbedBuilder();

        builder.WithTitle($"{SquadronName.ToUpper()} squadron info");
        builder.AddField(nameof(Tag), Tag);
        builder.AddField(nameof(Members), Members);
        builder.AddField(nameof(Owner), Owner);
        builder.AddField(nameof(Platform), Platform);
        builder.AddField("Creation Date", CreationDate.ToString(Constants.DateTimeFormat));
        builder.AddField(nameof(Power), Power);
        builder.AddField("Super Power", SuperPower);
        builder.AddField(nameof(Faction), Faction);
        builder.AddField("User Tags", UserTags);
        builder.AddField("Squad Id", SquadId);

        builder.WithFooter($"Updated: {UpdatedDate.ToString(Constants.DateTimeFormat)}");
        builder.WithColor(0, 49, 83);

        return builder.Build();
    }
}