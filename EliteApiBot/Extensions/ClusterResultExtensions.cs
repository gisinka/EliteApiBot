using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Model;

namespace EliteApiBot.Extensions
{
    public static class ClusterResultExtensions
    {
        public static void EnsureSuccess(this ClusterResult clusterResult)
        {
            if (clusterResult.Status is ClusterResultStatus.Success)
                return;

            throw new ClusterClientException($"Failed to request {clusterResult.Request.Url.AbsolutePath} with status {clusterResult.Status}");
        }
    }
}
