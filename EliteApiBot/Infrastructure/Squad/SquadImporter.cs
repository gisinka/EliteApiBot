using Discord;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadImporter
{
    public static async Task<IEnumerable<Embed>> GetSquadStrings(string tag, bool isFull = false)
    {
        if (!IsValidTag(tag))
            return new List<Embed> { Constants.InvalidTagEmbed };

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "ISIN Squad bot instance");

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), client, isFull);
        if (squadJsons == null)
            return new List<Embed> { Constants.ApiFaultEmbed };

        IEnumerable<ISquadInfo> squadInfos = isFull
            ? await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfoFull>>(squadJsons, Constants.JsonSerializerSettings))
            : await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Any()
            ? squadInfos.Select(x => x.GetEmbed())
            : new List<Embed> { Constants.NotExistingTagEmbed };
    }

    private static bool IsValidTag(string tag)
    {
        return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
    }
}