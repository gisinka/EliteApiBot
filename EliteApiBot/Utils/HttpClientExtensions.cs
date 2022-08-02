namespace EliteApiBot.Utils
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddHeaders(this HttpClient client)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "ISIN Squad bot instance");
            return client;
        }
    }
}
