﻿using CsvHelper;
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