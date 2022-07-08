using Discord;
using Elite_API_Discord.Model;
using Elite_API_Discord.Utils;
using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadBuilder
{
    public static async Task<IEnumerable<Embed>> GetSquadsEmbeds(string tag, bool isFull = false, bool isRussian = true)
    {
        if (!IsValidTag(tag))
            return new List<Embed> { EmbedFactory.InvalidTagEmbed };

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "ISIN Squad bot instance");

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), client, isFull);
        if (squadJsons == null)
            return new List<Embed> { EmbedFactory.ApiFaultEmbed };

        IEnumerable<ISquadInfo> squadInfos = isFull
            ? await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfoFull>>(squadJsons, Constants.JsonSerializerSettings))
            : await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Any()
            ? squadInfos.Select(x => x.BuildEmbed(isRussian))
            : new List<Embed> { EmbedFactory.NotExistingTagEmbed };
    }

    private static bool IsValidTag(string tag)
    {
        return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
    }
}