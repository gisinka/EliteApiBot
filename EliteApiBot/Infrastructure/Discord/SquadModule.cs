using Discord.Commands;
using EliteApiBot.Infrastructure.Squad;
using EliteApiBot.Utils;

namespace EliteApiBot.Infrastructure.Discord;

public class SquadModule : ModuleBase<SocketCommandContext>
{

    private readonly SquadBuilder squadBuilder;
    private readonly IEliteApiClient eliteApiClient;

    public SquadModule(SquadBuilder squadBuilder, IEliteApiClient eliteApiClient)
    {
        this.squadBuilder = squadBuilder;
        this.eliteApiClient = eliteApiClient;
    }

    [Command("squadfull")]
    [Summary("Printing full squad info by tag")]
    public async Task GetFullSquadStringAsync([Summary("Squad tag")] string tag)
    {
        var contents = await squadBuilder.GetSquadsEmbeds(tag, true);

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }

    [Command("squad")]
    [Summary("Printing squad info by tag")]
    public async Task GetSquadStringsAsync([Summary("Squad tag")] string tag)
    {
        var contents = await squadBuilder.GetSquadsEmbeds(tag);

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }

    [Command("cb")]
    [Summary("Printing carebear info by name")]
    public async Task GetPlayerStringAsync([Summary("Player name")] [Remainder] string name)
    {
        var player = await eliteApiClient.GetPlayerAsync(name);

        await ReplyAsync("", false, player is null ? EmbedFactory.NotExistingNameEmbed : player.BuildEmbed());
    }
}