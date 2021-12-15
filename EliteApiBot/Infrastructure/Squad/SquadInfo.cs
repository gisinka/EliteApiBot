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

    [JsonProperty("Power name")] public string Power { get; set; }

    [JsonProperty("Super power name")] public string SuperPower { get; set; }

    [JsonProperty("Faction name")] public string Faction { get; set; }

    public override string ToString()
    {
        const char newLine = '\n';
        return $"{nameof(SquadronName)}: {SquadronName}{newLine}{nameof(Tag)}: {Tag}{newLine}{nameof(Members)}: {Members}{newLine}{nameof(Owner)}: {Owner}{newLine}{nameof(Platform)}: {Platform}{newLine}{nameof(CreationDate)}: {CreationDate.ToString(Constants.DateTimeFormat)}{newLine}{nameof(Power)}: {Power}{newLine}{nameof(SuperPower)}: {SuperPower}{newLine}{nameof(Faction)}: {Faction}";
    }
}