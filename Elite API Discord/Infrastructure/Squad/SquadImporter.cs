using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad
{
    public class SquadImporter
    {
        public static async Task<string> GetSquadString(string tag)
        {
            var squadJson = await SquadRequester.Request(tag.ToUpperInvariant(), new HttpClient());
            var squadInfo = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJson).FirstOrDefault());

            return squadInfo?.ToString() ?? "Invalid tag";
        }
    }
}
