using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad
{
    public class SquadImporter
    {
        public static async Task<IEnumerable<string>> GetSquadString(string tag)
        {
            var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), new HttpClient());
            var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons));
            
            return squadInfos.Select(x => x.ToString());
        }
    }
}
