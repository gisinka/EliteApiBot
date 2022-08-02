using Discord;

namespace EliteApiBot.Utils
{
    public static class EmbedFactory
    {
        public static readonly Embed InvalidTagEmbed = new EmbedBuilder()
            .AddField("Status", "Tag is invalid")
            .WithFooter($"Обновлено: {DateTime.Now.ToString(Constants.DateTimeFormat)}")
            .WithColor(Color.Gold)
            .Build();

        public static readonly Embed NotExistingTagEmbed = new EmbedBuilder()
            .AddField("Status", "Tag does not exist")
            .WithFooter($"Обновлено: {DateTime.Now.ToString(Constants.DateTimeFormat)}")
            .WithColor(Color.Gold)
            .Build();

        public static readonly Embed NotExistingNameEmbed = new EmbedBuilder()
            .AddField("Status", "Name does not exist")
            .WithFooter($"Обновлено: {DateTime.Now.ToString(Constants.DateTimeFormat)}")
            .WithColor(Color.Gold)
            .Build();

        public static readonly Embed ApiFaultEmbed = new EmbedBuilder()
            .AddField("Status", "Небольшие технические шоколадки, упало апи, alert")
            .WithFooter($"Момент запроса: {DateTime.Now.ToString(Constants.DateTimeFormat)}")
            .WithColor(Color.DarkRed)
            .Build();
    }
}
