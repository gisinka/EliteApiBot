using EliteApiBot.Utils;

namespace EliteApiBot.Infrastructure.Squad;

public class SquadRequester
{
    private readonly HttpClient client;

    public SquadRequester(HttpClient client)
    {
        this.client = client;
    }

    public async Task<string?> Request(string tag,  bool isFull = false)
    {
        var urlFormat = isFull
            ? Constants.JsonLinkWithTagsResolve
            : Constants.JsonLinkWithoutTagsResolve;
        var url = string.Format(urlFormat, tag);

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var httpResponse = await client.SendAsync(request);

        if (!httpResponse.IsSuccessStatusCode)
            return null;

        using var sr = new StreamReader(await httpResponse.Content.ReadAsStreamAsync());

        if (httpResponse.Content.Headers.ContentType?.MediaType != "application/json")
            return null;

        return await sr.ReadToEndAsync();
    }
}