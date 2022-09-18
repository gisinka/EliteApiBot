using CsvHelper;
using Discord;
using EliteApiBot.Utils;

namespace EliteApiBot.Infrastructure.Player;

public class PlayerImporter
{
    private readonly HttpClient client;

    public PlayerImporter(HttpClient client)
    {
        this.client = client;
    }

    public async Task<Embed> GetNameStringsAsync(string name)
    {
        var sr = new StreamReader(await client.GetStreamAsync(Constants.CsvLink));
        using var csv = new CsvReader(sr, Constants.CsvConfiguration);
        var playersIterator = csv
            .EnumerateRecordsAsync(new Model.Player())
            .GetAsyncEnumerator();
        while (await playersIterator.MoveNextAsync())
        {
            if (playersIterator.Current is null)
                continue;

            if (string.Equals(playersIterator.Current.CMDR, name, StringComparison.InvariantCultureIgnoreCase))
                return playersIterator.Current.BuildEmbed();
        }

        return EmbedFactory.NotExistingNameEmbed;
    }
}