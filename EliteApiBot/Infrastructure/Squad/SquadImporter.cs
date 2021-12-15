using Newtonsoft.Json;

namespace Elite_API_Discord.Infrastructure.Squad
{
    public class SquadImporter
    {
        public static async Task<IEnumerable<string>> GetFullSquadStrings(string tag)
        {
            if (!IsValidTag(tag))
                return new List<string>() { "Tag is invalid" };

            var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), new HttpClient(), true);
            var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfoFull>>(squadJsons));
            
            return squadInfos.Count == 0 
                ? new List<string>() { "Tag does not exist" } 
                : squadInfos.Select(x => x.ToString());
        }

        public static async Task<IEnumerable<string>> GetSquadStrings(string tag)
        {
            if (!IsValidTag(tag))
                return new List<string>() { "Tag is invalid" };

            var squadJsons = await SquadRequester.Request(tag.ToUpperInvariant(), new HttpClient());
            var squadInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<SquadInfo>>(squadJsons));

            return squadInfos.Count == 0
                ? new List<string>() { "Tag does not exist" }
                : squadInfos.Select(x => x.ToString());
        }

        private static bool IsValidTag(string tag)
        {
            return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
        }
    }
}
