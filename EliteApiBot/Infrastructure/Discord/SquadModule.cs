using Discord;
using Discord.Commands;
using EliteApiBot.Infrastructure.Squad;
using EliteApiBot.Model;
using EliteApiBot.Utils;

namespace EliteApiBot.Infrastructure.Discord;

public class SquadModule : ModuleBase<SocketCommandContext>
{

    private readonly IEliteApiClient eliteApiClient;

    public SquadModule(IEliteApiClient eliteApiClient)
    {
        this.eliteApiClient = eliteApiClient;
    }

    [Command("squadfull")]
    [Summary("Printing full squad info by tag")]
    public async Task GetFullSquadStringAsync([Summary("Squad tag")] string tag)
    {
        var contents = await GetSquadsEmbeds(tag, true);

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }

    [Command("squad")]
    [Summary("Printing squad info by tag")]
    public async Task GetSquadStringsAsync([Summary("Squad tag")] string tag)
    {
        var contents = await GetSquadsEmbeds(tag);

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }

    [Command("cb")]
    [Summary("Printing carebear info by name")]
    public async Task GetPlayerStringAsync([Summary("Player name")] [Remainder] string name)
    {
        var player = await eliteApiClient.GetPlayerAsync(name);

        await ReplyAsync("", false, player is null ? EmbedFactory.NotExistingNameEmbed : player.BuildEmbed());
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