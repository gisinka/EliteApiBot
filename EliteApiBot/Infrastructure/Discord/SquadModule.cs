using Discord.Commands;
using Elite_API_Discord.Infrastructure.Squad;

namespace Elite_API_Discord.Infrastructure.Discord;

public class SquadModule : ModuleBase<SocketCommandContext>
{
    [Command("squadfull")]
    [Summary("Printing full squad info by tag")]
    public async Task GetFullSquadStringAsync([Summary("Squad tag")] string tag)
    {
        var contents = await SquadImporter.GetFullSquadStrings(tag);

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }

    [Command("squad")]
    [Summary("Printing squad info by tag")]
    public async Task GetSquadStringsAsync([Summary("Squad tag")] string tag)
    {
        var contents = await SquadImporter.GetSquadStrings(tag);

        await Task.WhenAll(contents.Select(content => ReplyAsync("", false, content)));
    }
}