using System.Text;
using CsvHelper;
using EliteApiBot.Extensions;
using EliteApiBot.Model;
using EliteApiBot.Utils;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Core.Strategies;
using Vostok.Clusterclient.Core.Topology;
using Vostok.Clusterclient.Transport;
using Vostok.Logging.Abstractions;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace EliteApiBot.Infrastructure.Squad
{
    public class EliteApiClient : IEliteApiClient
    {
        private readonly IClusterClient clusterClient;
        private readonly MemoryCache playersCache;
        private readonly BotConfiguration botConfiguration;

        public EliteApiClient(IConfigurationProvider configurationProvider, ILog log)
        {
            botConfiguration = configurationProvider.Get<BotConfiguration>();
            clusterClient = new ClusterClient(log, GetSetup());
            playersCache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(10),
                SizeLimit = botConfiguration.PlayersCacheSizeLimit
            });
        }

        public async Task<IReadOnlyCollection<SquadInfo>> GetSquadInfoAsync(string tag)
        {
            var uri = string.Format(Constants.JsonLinkWithoutTagsResolve, tag);
            var response = await GetAsync(new Uri(uri, UriKind.Relative));
            response.EnsureSuccessStatusCode();
            var responseBody = Encoding.UTF8.GetString(await response.GetBytesAsync());
            return JsonConvert.DeserializeObject<List<SquadInfo>>(responseBody, Constants.JsonSerializerSettings) as IReadOnlyCollection<SquadInfo>
                   ?? Array.Empty<SquadInfo>();
        }

        public async Task<IReadOnlyCollection<FullSquadInfo>> GetFullSquadInfoAsync(string tag)
        {
            var uri = string.Format(Constants.JsonLinkWithTagsResolve, tag);
            var response = await GetAsync(new Uri(uri, UriKind.Relative));
            response.EnsureSuccessStatusCode();
            var responseBody = Encoding.UTF8.GetString(await response.GetBytesAsync());
            return JsonConvert.DeserializeObject<List<FullSquadInfo>>(responseBody, Constants.JsonSerializerSettings) as IReadOnlyCollection<FullSquadInfo>
                   ?? Array.Empty<FullSquadInfo>();
        }

        public async Task<Player?> GetPlayerAsync(string name)
        {
            var invariantName = name.ToLowerInvariant();
            if (playersCache.TryGetValue(invariantName, out var player))
                return (Player) player;

            return await UpdateCache(invariantName);
        }

        public async Task<Player?> UpdateCache(string name)
        {
            var response = await GetAsync(new Uri(Constants.CsvLink, UriKind.Absolute));
            response.EnsureSuccessStatusCode();

            var result = (Player) null;
            var sr = new StreamReader(response.GetStream());
            using var csv = new CsvReader(sr, Constants.CsvConfiguration);
            var playersIterator = csv
                .EnumerateRecordsAsync(new Player())
                .GetAsyncEnumerator();

            while (await playersIterator.MoveNextAsync())
            {
                if (playersIterator.Current is null)
                    continue;
                var currentPlayer = playersIterator.Current;
                playersCache.Set(currentPlayer.CMDR.ToLowerInvariant(), currentPlayer, currentPlayer.CreatEntryOptions(botConfiguration.PlayersCacheExpirationTimeout));

                if (string.Equals(playersIterator.Current.CMDR, name, StringComparison.InvariantCultureIgnoreCase))
                    result = playersIterator.Current;
            }

            if (result is not null) 
                return result;

            var entryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(botConfiguration.PlayersCacheExpirationTimeout)
                .SetSize(0);
            playersCache.Set(name, result, entryOptions);

            return result;
        }


        private async Task<Response> GetAsync(Uri uri)
        {
            var clusterResult = await clusterClient.SendAsync(
                new Request("GET", uri)
                .WithHeader("User-Agent", "ISIN Squad bot instance"));
            clusterResult.EnsureSuccess();

            return clusterResult.Response;
        }


        private static ClusterClientSetup GetSetup()
        {
            return config =>
            {
                config.SetupUniversalTransport(new UniversalTransportSettings
                {
                    UseResponseStreaming = length => length > 1024 * 1024
                });
                config.ClusterProvider = new FixedClusterProvider(Constants.ApiUrl);
                config.DefaultRequestStrategy = new SingleReplicaRequestStrategy();
                config.DefaultTimeout = TimeSpan.FromSeconds(30);
            };
        }
    }
}
