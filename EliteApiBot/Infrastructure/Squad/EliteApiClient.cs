using EliteApiBot.Model;
using System;
using System.Text;
using EliteApiBot.Extensions;
using EliteApiBot.Utils;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Model;
using Vostok.Logging.Abstractions;
using Vostok.Clusterclient.Core.Strategies;
using Vostok.Clusterclient.Core.Topology;
using Vostok.Clusterclient.Transport;
using Newtonsoft.Json;
using CsvHelper;
using Microsoft.Extensions.Caching.Memory;

namespace EliteApiBot.Infrastructure.Squad
{
    public class EliteApiClient : IEliteApiClient
    {
        private readonly IClusterClient clusterClient;
        private readonly MemoryCache playersCache;

        public EliteApiClient(ILog log)
        {
            clusterClient = new ClusterClient(log, GetSetup());
            playersCache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(10),
                SizeLimit = 1024 * 1024
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

        public async Task<Model.Player?> GetPlayerAsync(string name)
        {
            var invariantName = name.ToLowerInvariant();
            return await playersCache.GetOrCreateAsync(invariantName, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(1);
                entry.SetSize(sizeof(char) * invariantName.Length);
                var player = await GetPlayerAsyncInternal(name);
                return player;
            });
        }

        public async Task<Model.Player?> GetPlayerAsyncInternal(string name)
        {
            var response = await GetAsync(new Uri(Constants.CsvLink, UriKind.Absolute));
            response.EnsureSuccessStatusCode();

            var sr = new StreamReader(response.GetStream());
            using var csv = new CsvReader(sr, Constants.CsvConfiguration);
            var playersIterator = csv
                .EnumerateRecordsAsync(new Model.Player())
                .GetAsyncEnumerator();
            while (await playersIterator.MoveNextAsync())
            {
                if (playersIterator.Current is null)
                    continue;

                if (string.Equals(playersIterator.Current.CMDR, name, StringComparison.InvariantCultureIgnoreCase))
                    return playersIterator.Current;
            }

            return null;
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
