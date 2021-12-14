using Discord.Commands;
using Elite_API_Discord.Infrastructure.Squad;

namespace Elite_API_Discord.Infrastructure.Discord
{
    public class SquadModule : ModuleBase<SocketCommandContext>
    {
        [Command("squad")]
        [Summary("Printing squad info by tag")]
        public async Task SquareAsync([Summary("Squad tag")] string tag)
        {
            var content = await SquadImporter.GetSquadString(tag);
            await ReplyAsync(content);
        }
    }
}
