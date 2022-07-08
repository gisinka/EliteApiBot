using Discord;

namespace Elite_API_Discord.Model
{
    public interface ISquadInfo
    {
        Embed BuildEmbed(bool isRussian);
    }
}
