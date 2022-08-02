using Discord;
using EliteApiBot.Model;
using EliteApiBot.Utils;
using Newtonsoft.Json;

namespace EliteApiBot.Infrastructure.Squad;

public class SquadBuilder
{
    private readonly SquadRequester squadRequester;

    public SquadBuilder(SquadRequester squadRequester)
    {
        this.squadRequester = squadRequester;
    }

    public async Task<IEnumerable<Embed>> GetSquadsEmbeds(string tag, bool isFull = false, bool isRussian = true)
    {
        if (!IsValidTag(tag))
            return new List<Embed> { EmbedFactory.InvalidTagEmbed };

        var squadJsons = await squadRequester.Request(tag.ToUpperInvariant(), isFull);
        if (squadJsons == null)
            return new List<Embed> { EmbedFactory.ApiFaultEmbed };

        var squadInfos = await SerializeSquadInfos(squadJsons, isFull);

        return BuildEmbeds(squadInfos, isRussian);
    }

    private static bool IsValidTag(string tag)
    {
        return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
    }

    private static async Task<List<ISquadInfo>> SerializeSquadInfos(string? squadJsons, bool isFull = false)
    {
        IEnumerable<ISquadInfo>? squadInfos = isFull
            ? await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfoFull>>(squadJsons, Constants.JsonSerializerSettings))
            : await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons, Constants.JsonSerializerSettings));

        return squadInfos is null
            ? new List<ISquadInfo>()
            : squadInfos.ToList();

    }

    private static IEnumerable<Embed> BuildEmbeds(List<ISquadInfo> squadInfos, bool isRussian = true)
    {
        return squadInfos.Any()
            ? squadInfos.Select(x => x.BuildEmbed(isRussian))
            : new List<Embed> { EmbedFactory.NotExistingTagEmbed };
    }
}