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
        var tasks = new List<Task>();

        foreach (var content in contents)
        {
            var task = new Task(() => ReplyAsync(content));
            task.Start();
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());
    }

    [Command("squad")]
    [Summary("Printing squad info by tag")]
    public async Task GetSquadStringsAsync([Summary("Squad tag")] string tag)
    {
        var contents = await SquadImporter.GetSquadStrings(tag);
        var tasks = new List<Task>();

        foreach (var content in contents)
        {
            var task = new Task(() => ReplyAsync(content));
            task.Start();
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());
    }
}