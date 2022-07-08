using Discord;
using Elite_API_Discord.Utils;
using Newtonsoft.Json;

namespace Elite_API_Discord.Model;

public class SquadInfoFull : ISquadInfo
{
    [JsonProperty("Squadron name")] public string SquadronName { get; set; } = "N/D";

    [JsonProperty("Tag")] public string Tag { get; set; } = "N/D";

    [JsonProperty("Members")] public short Members { get; set; }

    [JsonProperty("Owner")] public string Owner { get; set; } = "N/D";

    [JsonProperty("Platform")] public string Platform { get; set; } = "N/D";

    [JsonProperty("Created UTC")] public DateTime CreationDate { get; set; } = DateTime.Now;

    [JsonProperty("Power name")] public string Power { get; set; } = "N/D";

    [JsonProperty("Super power name")] public string SuperPower { get; set; } = "N/D";

    [JsonProperty("Faction name")] public string Faction { get; set; } = "N/D";

    [JsonProperty("User tags")] public string UserTags { get; set; } = "N/D";

    [JsonProperty("Updated UTC")] public DateTime UpdatedDate { get; set; } = DateTime.Now;

    [JsonProperty("squad_id")] public long SquadId { get; set; } = long.MinValue;

    [JsonProperty("motd")] public string Motd { get; set; } = "N/D";

    public Embed BuildEmbed(bool isRussian = true)
    {
        return isRussian
            ? new EmbedBuilder()
                .WithTitle($"Информация о эскадрилье {SquadronName.ToUpper()}")
                .AddField("Тег эскадры", $"[{Tag}]({string.Format(Constants.ExtendedLink, Tag)})")
                .AddField("Количество членов", Members)
                .AddField("Владелец", Owner)
                .AddField("Платформа", Platform)
                .AddField("Дата создания", CreationDate.ToString(Constants.DateTimeFormat))
                .AddField("Галактическая держава", Power)
                .AddField("Сверхдержава", SuperPower)
                .AddField("Игровая фракция", Faction)
                .AddField("Пользовательские теги", UserTags)
                .AddField("Девиз", Motd)
                .AddField("Id эскадрильи", SquadId)
                .WithFooter($"Обновлено: {UpdatedDate.ToString(Constants.DateTimeFormat)}")
                .WithColor(0, 49, 83)
                .Build()
            : new EmbedBuilder()
                .WithTitle($"{SquadronName.ToUpper()} squadron info")
                .AddField(nameof(Tag), $"[{Tag}]({string.Format(Constants.ExtendedLink, Tag)})")
                .AddField(nameof(Members), Members)
                .AddField(nameof(Owner), Owner)
                .AddField(nameof(Platform), Platform)
                .AddField("Creation Date", CreationDate.ToString(Constants.DateTimeFormat))
                .AddField(nameof(Power), Power)
                .AddField("Super Power", SuperPower)
                .AddField(nameof(Faction), Faction)
                .AddField("User Tags", UserTags)
                .AddField(nameof(Motd), Motd)
                .AddField("Squad Id", SquadId)
                .WithFooter($"Updated: {UpdatedDate.ToString(Constants.DateTimeFormat)}")
                .WithColor(0, 49, 83)
                .Build();
    }
}