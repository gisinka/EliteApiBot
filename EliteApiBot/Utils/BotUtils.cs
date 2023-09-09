using Discord;
using EliteApiBot.Model;

namespace EliteApiBot.Utils
{
    internal static class BotUtils
    {
        public static bool IsValidTag(string tag)
        {
            return tag.Length == 4 && tag.All(char.IsLetterOrDigit);
        }

        public static IEnumerable<Embed> BuildEmbeds(IReadOnlyCollection<ISquadInfo>? squadInfos, bool isRussian = true)
        {
            if (squadInfos is null)
                return new List<Embed> { EmbedFactory.ApiFaultEmbed };

            return squadInfos.Any()
                ? squadInfos.Select(x => x.ToEmbed(isRussian))
                : Enumerable.Repeat(EmbedFactory.NotExistingTagEmbed, 1);
        }
    }
}
