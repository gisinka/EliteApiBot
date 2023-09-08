using Vostok.Clusterclient.Core.Model;

namespace EliteApiBot.Extensions
{
    internal static class ResponseExtensions
    {
        public static async Task<byte[]> GetBytesAsync(this Response response)
        {
            return response.HasContent
                ? response.Content.ToArray()
                : await response.Stream.ReadFullyAsync();
        }

        public static Stream GetStream(this Response response)
        {
            return response.HasContent
                ? response.Content.ToMemoryStream()
                : response.Stream;
        }
    }
}
