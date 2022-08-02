using Discord;

namespace EliteApiBot.Model
{
    public interface ISquadInfo
    {
        Embed BuildEmbed(bool isRussian);
    }
}
