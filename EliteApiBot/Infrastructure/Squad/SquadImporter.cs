using Discord;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadImporter
{
    public static async Task<IEnumerable<string>> GetFullSquadStrings(string tag)
    {
        if (!IsValidTag(tag))
            return new List<string> { "Tag is invalid" };

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "ISIN Squad bot instance");

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), client, true);
        if (squadJsons == null)
            return new List<string> { "Небольшие технические шоколадки, упало апи, alert" };

        var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfoFull>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Count == 0
            ? new List<string> { "Tag does not exist" }
            : squadInfos.Select(x => x.ToString());
    }

    public static async Task<IEnumerable<Embed>> GetSquadStrings(string tag)
    {
        if (!IsValidTag(tag))
            return new List<Embed> { new EmbedBuilder() {Fields = new List<EmbedFieldBuilder>() { new EmbedFieldBuilder() { IsInline = false, Name = "Status", Value = "Tag is invalid"}} }.Build()};

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "ISIN Squad bot instance");

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), client);
        if (squadJsons == null)
            return new List<Embed> { new EmbedBuilder() { Fields = new List<EmbedFieldBuilder>() { new EmbedFieldBuilder() { IsInline = false, Name = "Status", Value = "Небольшие технические шоколадки, упало апи, alert" } } }.Build() };

        var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Count == 0
            ? new List<Embed> { new EmbedBuilder() {Fields = new List<EmbedFieldBuilder>() { new EmbedFieldBuilder() { IsInline = false, Name = "Status", Value = "Tag does not exist"}} }.Build()}
            : squadInfos.Select(x => x.GetEmbed());
    }

    private static bool IsValidTag(string tag)
    {
        return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
    }
}