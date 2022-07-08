using System.Globalization;
using System.Net;
using CsvHelper;
using CsvHelper.Configuration;
using Discord;
using Elite_API_Discord.Utils;

namespace Elite_API_Discord.Infrastructure.Player;

public class PlayerImporter
{
    public static async Task<Embed> GetNameStringsAsync(string name)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Host = "gitea.demb.design";

        var sr = new StreamReader(await client.GetStreamAsync(Constants.CsvLink));
        using var csv = new CsvReader(sr, Constants.CsvConfiguration);
        var playersIterator = csv
            .EnumerateRecordsAsync(new Player())
            .GetAsyncEnumerator();
        while (await playersIterator.MoveNextAsync())
        {
            if (playersIterator.Current is null)
                continue;

            if (string.Equals(playersIterator.Current.CMDR, name, StringComparison.InvariantCultureIgnoreCase))
                return GetEmbed(playersIterator.Current);
        }

        return EmbedFactory.NotExistingNameEmbed;
    }

    private static Embed GetEmbed(Player player)
    {
        return new EmbedBuilder()
            .WithTitle($"Информация о игроке {player.CMDR}")
            .AddField("Игровой ник", player.CMDR)
            .AddField("Эскадра", player.Squadron)
            .AddField("Тег эскадры", player.SQID)
            .WithFooter($"Обновлено: {DateTime.Now.ToString(Constants.DateTimeFormat)}")
            .WithColor(127, 199, 255)
            .Build();
    }
}