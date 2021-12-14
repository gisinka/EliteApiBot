using Discord.Commands;
using Elite_API_Discord.Infrastructure.Squad;

namespace Elite_API_Discord.Infrastructure.Discord;

public class SquadModule : ModuleBase<SocketCommandContext>
{
    [Command("squad")]
    [Summary("Printing squad info by tag")]
    public async Task SquareAsync([Summary("Squad tag")] string tag)
    {
        var contents = await SquadImporter.GetSquadString(tag);
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