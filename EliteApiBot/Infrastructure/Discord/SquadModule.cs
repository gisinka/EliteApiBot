using Discord.Commands;
using EliteApiBot.Infrastructure.Squad;
using EliteApiBot.Utils;
using SummaryAttribute = Discord.Commands.SummaryAttribute;

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
        if (!BotUtils.IsValidTag(tag))
        {
            await ReplyAsync(string.Empty, false, EmbedFactory.InvalidTagEmbed);
            return;
        }

        var contents = BotUtils.BuildEmbeds(await eliteApiClient.GetFullSquadInfoAsync(tag));

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }

    [Command("squad")]
    [Summary("Printing squad info by tag")]
    public async Task GetSquadStringsAsync([Summary("Squad tag")] string tag)
    {
        if (!BotUtils.IsValidTag(tag))
        {
            await ReplyAsync(string.Empty, false, EmbedFactory.InvalidTagEmbed);
            return;
        }

        var contents = BotUtils.BuildEmbeds(await eliteApiClient.GetSquadInfoAsync(tag));

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }

    [Command("cb")]
    [Summary("Printing carebear info by name")]
    public async Task GetPlayerStringAsync([Summary("Player name")] [Remainder] string name)
    {
        var player = await eliteApiClient.GetPlayerAsync(name);

        await ReplyAsync(string.Empty, false, player is null ? EmbedFactory.NotExistingNameEmbed : player.BuildEmbed());
    }
}