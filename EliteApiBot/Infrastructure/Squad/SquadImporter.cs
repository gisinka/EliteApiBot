using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadImporter
{
    public static async Task<IEnumerable<string>> GetFullSquadStrings(string tag)
    {
        if (!IsValidTag(tag))
            return new List<string> { "Tag is invalid" };

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), new HttpClient(), true);
        if (squadJsons == null)
            return new List<string> { "Небольшие технические шоколадки, упало апи, alert" };

        var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfoFull>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Count == 0
            ? new List<string> { "Tag does not exist" }
            : squadInfos.Select(x => x.ToString());
    }

    public static async Task<IEnumerable<string>> GetSquadStrings(string tag)
    {
        if (!IsValidTag(tag))
            return new List<string> { "Tag is invalid" };

        var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), new HttpClient());
        if (squadJsons == null)
            return new List<string> { "Небольшие технические шоколадки, упало апи, alert" };

        var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos.Count == 0
            ? new List<string> { "Tag does not exist" }
            : squadInfos.Select(x => x.ToString());
    }

    private static bool IsValidTag(string tag)
    {
        return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
    }
}