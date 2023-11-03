using Discord.Commands;
using Discord.Interactions;
using EliteApiBot.Infrastructure.Squad;
using EliteApiBot.Utils;
using SummaryAttribute = Discord.Commands.SummaryAttribute;

namespace EliteApiBot.Infrastructure.Discord;

public class SquadInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IEliteApiClient eliteApiClient;

    public SquadInteractionModule(IEliteApiClient eliteApiClient)
    {
        this.eliteApiClient = eliteApiClient;
    }

    [SlashCommand("squadfull", "Printing full squad info by tag")]
    public async Task GetFullSquadStringAsync([Summary("Squad tag")] string tag)
    {
        if (!BotUtils.IsValidTag(tag))
        {
            await RespondAsync(string.Empty, new[] { EmbedFactory.InvalidTagEmbed });
            return;
        }

        var contents = BotUtils.BuildEmbeds(await eliteApiClient.GetFullSquadInfoAsync(tag)).ToArray();

        await RespondAsync(string.Empty, contents);
    }

    [SlashCommand("squad", "Printing squad info by tag")]
    public async Task GetSquadStringsAsync([Summary("Squad tag")] string tag)
    {
        if (!BotUtils.IsValidTag(tag))
        {
            await RespondAsync(string.Empty, new []{ EmbedFactory.InvalidTagEmbed });
            return;
        }

        var contents = BotUtils.BuildEmbeds(await eliteApiClient.GetSquadInfoAsync(tag)).ToArray();

        await RespondAsync(string.Empty, contents);
    }

    [SlashCommand("cb", "Printing carebear info by name")]
    public async Task GetPlayerStringAsync([Summary("Player name")][Remainder] string name)
    {
        var player = await eliteApiClient.GetPlayerAsync(name);

        await RespondAsync(string.Empty, new[] { player is null ? EmbedFactory.NotExistingNameEmbed : player.BuildEmbed() });
    }
}