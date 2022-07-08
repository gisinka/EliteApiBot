using Discord;

namespace Elite_API_Discord.Infrastructure.Squad
{
    public interface ISquadInfo
    {
        Embed BuildEmbed(bool isRussian);
    }
}
