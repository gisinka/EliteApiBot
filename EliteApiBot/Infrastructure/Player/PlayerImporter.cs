using System.Globalization;
using System.Net;
using CsvHelper;
using CsvHelper.Configuration;
using Discord;

namespace Elite_API_Discord.Infrastructure.Player;

public class PlayerImporter
{
    public static async Task<Embed> GetNameStringsAsync(string name)
    {
        var req = (HttpWebRequest)WebRequest.Create(Constants.CsvLink);
        req.Host = "gitea.demb.design";
        var resp = (HttpWebResponse) await req.GetResponseAsync();
        var sr = new StreamReader(resp.GetResponseStream());
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true
        };

        using var csv = new CsvReader(sr, configuration);
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

        return Constants.NotExistingNameEmbed;
    }

    private static Embed GetEmbed(Player player)
    {
        var builder = new EmbedBuilder();

        builder.WithTitle($"Информация о игроке {player.CMDR}");
        builder.AddField("Игровой ник", player.CMDR);
        builder.AddField("Эскадра", player.Squadron);
        builder.AddField("Тег эскадры", player.SQID);

        builder.WithFooter($"Обновлено: {DateTime.Now.ToString(Constants.DateTimeFormat)}");
        builder.WithColor(127, 199, 255);

        return builder.Build();
    }
}