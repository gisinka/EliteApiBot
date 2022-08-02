using Discord;
using EliteApiBot.Utils;

namespace EliteApiBot.Model;

public class Player
{
    public string CMDR { get; set; }
    public string Squadron { get; set; }
    public string SQID { get; set; }

    public Embed BuildEmbed()
    {
        return new EmbedBuilder()
            .WithTitle($"Информация о игроке {CMDR}")
            .AddField("Игровой ник", CMDR)
            .AddField("Эскадра", Squadron)
            .AddField("Тег эскадры", SQID)
            .WithFooter($"Обновлено: {DateTime.Now.ToString(Constants.DateTimeFormat)}")
            .WithColor(127, 199, 255)
            .Build();
    }
}