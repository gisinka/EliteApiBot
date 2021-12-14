using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadRequester
{
    internal static async Task<string> Request(string tag, HttpClient httpClient)
    {
        var url = $"https://sapi.demb.design/api/squads/now/by-tag/extended/{tag}?resolve_tags=true";
        using var httpResponse = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        httpResponse.EnsureSuccessStatusCode();

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