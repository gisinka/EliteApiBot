using Discord.Commands;
using EliteApiBot.Infrastructure.Player;
using EliteApiBot.Infrastructure.Squad;

namespace EliteApiBot.Infrastructure.Discord;

public class SquadModule : ModuleBase<SocketCommandContext>
{

    private readonly SquadBuilder squadBuilder;
    private readonly PlayerImporter playerImporter;

    public SquadModule(SquadBuilder squadBuilder, PlayerImporter playerImporter)
    {
        this.squadBuilder = squadBuilder;
        this.playerImporter = playerImporter;
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
        var content = await playerImporter.GetNameStringsAsync(name);

        await ReplyAsync("", false, content);
    }
}