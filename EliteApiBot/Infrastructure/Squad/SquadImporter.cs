using Discord;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadImporter
{
    public static async Task<IEnumerable<Embed>> GetFullSquadStrings(string tag)
    {
        if (!IsValidTag(tag))
            return new List<Embed> { Constants.InvalidTagEmbed };

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "ISIN Squad bot instance");

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), client, true);

        if (squadJsons == null)
            return new List<Embed> { Constants.ApiFaultEmbed };

        var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfoFull>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Count == 0
            ? new List<Embed> { Constants.NotExistingTagEmbed }
            : squadInfos.Select(x => x.GetEmbed());
    }

    public static async Task<IEnumerable<Embed>> GetSquadStrings(string tag)
    {
        if (!IsValidTag(tag))
            return new List<Embed> { Constants.InvalidTagEmbed };

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "ISIN Squad bot instance");

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), client);

        if (squadJsons == null)
            return new List<Embed> { Constants.ApiFaultEmbed };

        var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Count == 0
            ? new List<Embed> { Constants.NotExistingTagEmbed }
            : squadInfos.Select(x => x.GetEmbed());
    }

    private static bool IsValidTag(string tag)
    {
        return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
    }
}