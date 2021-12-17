using Discord;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadInfo
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

    public override string ToString()
    {
        const char newLine = '\n';
        return $"{nameof(SquadronName)}: {SquadronName}{newLine}{nameof(Tag)}: {Tag}{newLine}{nameof(Members)}: {Members}{newLine}{nameof(Owner)}: {Owner}{newLine}{nameof(Platform)}: {Platform}{newLine}{nameof(CreationDate)}: {CreationDate.ToString(Constants.DateTimeFormat)}{newLine}{nameof(Power)}: {Power}{newLine}{nameof(SuperPower)}: {SuperPower}{newLine}{nameof(Faction)}: {Faction}";
    }

    public Embed GetEmbed()
    {
        var builder = new EmbedBuilder();

        builder.WithTitle($"{SquadronName.ToUpper()} squadron info");
        builder.AddField(nameof(Tag), Tag);
        builder.AddField(nameof(Members), Members);
        builder.AddField(nameof(Owner), Owner);
        builder.AddField(nameof(Platform), Platform);
        builder.AddField(nameof(CreationDate), CreationDate.ToString(Constants.DateTimeFormat));
        builder.AddField(nameof(Power), Power);
        builder.AddField(nameof(SuperPower), SuperPower);
        builder.AddField(nameof(Faction), Faction);

        return builder.Build();
    }
}