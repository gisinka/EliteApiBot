using System.Text.Json;

namespace Elite_API_Discord.Infrastructure.Squad;

public class SquadRequester
{
    internal static async Task<string?> Request(string tag, HttpClient httpClient, bool isFull = false)
    {
        var url = isFull 
            ? $"https://146.66.200.10/api/squads/now/by-tag/extended/{tag}?resolve_tags=true&pretty_keys=true"
            : $"https://146.66.200.10/api/squads/now/by-tag/extended/{tag}?pretty_keys=true";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Host = "sapi.demb.design";
        using var httpResponse = await httpClient.SendAsync(request);

        if (!httpResponse.IsSuccessStatusCode)
            return null;

        using var sr = new StreamReader(await httpResponse.Content.ReadAsStreamAsync());

        if (httpResponse.Content.Headers.ContentType?.MediaType != "application/json")
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