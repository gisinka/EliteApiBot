﻿using System.Globalization;
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

    public override string ToString()
    {
        const char newLine = '\n';
        return $"{nameof(SquadronName)}: {SquadronName}{newLine}{nameof(Tag)}: {Tag}{newLine}{nameof(Members)}: {Members}{newLine}{nameof(Owner)}: {Owner}{newLine}{nameof(Platform)}: {Platform}{newLine}{nameof(CreationDate)}: {CreationDate.ToString(Constants.DateTimeFormat)}{newLine}{nameof(Power)}: {Power}{newLine}{nameof(SuperPower)}: {SuperPower}{newLine}{nameof(Faction)}: {Faction}{newLine}{nameof(UserTags)}:{newLine}{UserTags.TrimEnd('\n')}{newLine}{nameof(UpdatedDate)}: {UpdatedDate.ToString(Constants.DateTimeFormat)}{newLine}{nameof(SquadId)}: {SquadId}";
    }

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
        builder.AddField("Updated Date", UpdatedDate.ToString(Constants.DateTimeFormat));
        builder.AddField("Squad Id", SquadId);

        builder.WithDescription("Updated: ");
        builder.WithTimestamp(UpdatedDate);
        builder.WithColor(0, 49, 83);

        return builder.Build();
    }
}