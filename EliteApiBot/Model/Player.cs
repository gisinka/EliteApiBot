using Discord;
using EliteApiBot.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace EliteApiBot.Model;

public class Player
{
    public string CMDR { get; set; }
    public string Squadron { get; set; }
    public string SQID { get; set; }
    public DateTime UpdatedTime { get; set; } = DateTime.Now;

    public Embed BuildEmbed()
    {
        return new EmbedBuilder()
            .WithTitle($"Информация о игроке {CMDR}")
            .AddField("Игровой ник", CMDR)
            .AddField("Эскадра", Squadron)
            .AddField("Тег эскадры", SQID)
            .WithFooter($"Обновлено: {UpdatedTime.ToString(Constants.DateTimeFormat)}")
            .WithColor(127, 199, 255)
            .Build();
    }

    public MemoryCacheEntryOptions CreatEntryOptions(TimeSpan expirationTimeout)
    {
        return new MemoryCacheEntryOptions()
            .SetSlidingExpiration(expirationTimeout)
            .SetSize(sizeof(char) * (CMDR.Length + SQID.Length + Squadron.Length) + 8);
    }
}