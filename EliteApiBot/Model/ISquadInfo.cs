using Discord;

namespace EliteApiBot.Model;

public interface ISquadInfo
{
    Embed ToEmbed(bool isRussian);
}