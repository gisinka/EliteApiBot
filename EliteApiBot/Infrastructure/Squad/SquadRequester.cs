using System.Text.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadRequester
{
    internal static async Task<string?> Request(string tag, HttpClient httpClient, bool isFull = false)
    {
        var url = isFull 
            ? $"https://sapi.demb.design/api/squads/now/by-tag/extended/{tag}?resolve_tags=true&pretty_keys=true"
            : $"https://sapi.demb.design/api/squads/now/by-tag/extended/{tag}?pretty_keys=true";
        using var httpResponse = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

        if (!httpResponse.IsSuccessStatusCode)
            return null;

        using var sr = new StreamReader(await httpResponse.Content.ReadAsStreamAsync());

        if (httpResponse.Content is not object || httpResponse.Content.Headers.ContentType.MediaType != "application/json")
            return null;

        try
        {
            return await sr.ReadToEndAsync();
        }
        catch (JsonException)
        {
            return null;
        }
    }
}