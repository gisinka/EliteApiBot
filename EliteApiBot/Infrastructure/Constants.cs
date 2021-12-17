using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure
{
    public static class Constants
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}
