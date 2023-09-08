using Discord;
using EliteApiBot.Model;
using EliteApiBot.Utils;

namespace EliteApiBot.Infrastructure.Squad;

public class SquadBuilder
{
    private readonly IEliteApiClient eliteApiClient;

    public SquadBuilder(IEliteApiClient eliteApiClient)
    {
        this.eliteApiClient = eliteApiClient;
    }

    public async Task<IEnumerable<Embed>> GetSquadsEmbeds(string tag, bool isFull = false, bool isRussian = true)
    {
        if (!IsValidTag(tag))
            return new List<Embed> { EmbedFactory.InvalidTagEmbed };

        return isFull
            ? BuildEmbeds(await eliteApiClient.GetFullSquadInfoAsync(tag))
            : BuildEmbeds(await eliteApiClient.GetSquadInfoAsync(tag));
    }

    private static bool IsValidTag(string tag)
    {
        return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
    }

    private static IEnumerable<Embed> BuildEmbeds(IReadOnlyCollection<ISquadInfo>? squadInfos, bool isRussian = true)
    {
        if (squadInfos is null)
            return new List<Embed> { EmbedFactory.ApiFaultEmbed };

        return squadInfos.Any()
            ? squadInfos.Select(x => x.ToEmbed(isRussian))
            : Enumerable.Repeat(EmbedFactory.NotExistingTagEmbed, 1);
    }
}