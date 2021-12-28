using Discord;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadInfo
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

    [JsonProperty("Updated UTC")] public DateTime UpdatedDate { get; set; } = DateTime.Now;

    public Embed GetEmbed()
    {
        var builder = new EmbedBuilder();

        builder.WithTitle($"Информация о эскадрилье {SquadronName.ToUpper()}");
        builder.AddField("Тег эскадры", Tag);
        builder.AddField("Количество членов", Members);
        builder.AddField("Владелец", Owner);
        builder.AddField("Платформа", Platform);
        builder.AddField("Дата создания", CreationDate.ToString(Constants.DateTimeFormat));
        builder.AddField("Галактическая держава", Power);
        builder.AddField("Сверхдержава", SuperPower);
        builder.AddField("Игровая фракция", Faction);

        builder.WithFooter($"Обновлено: {UpdatedDate.ToString(Constants.DateTimeFormat)}");
        builder.WithColor(0, 49, 83);

        return builder.Build();
    }

    public Embed GetEmbedEn()
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

        builder.WithFooter($"Updated: {UpdatedDate.ToString(Constants.DateTimeFormat)}");
        builder.WithColor(0, 49, 83);

        return builder.Build();
    }
}