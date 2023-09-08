using System.Buffers;

namespace EliteApiBot.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<byte[]> ReadFullyAsync(this Stream stream, int bufferSize = 4096)
        {
            if (stream is MemoryStream memoryStream)
                return memoryStream.ToArray();

            var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            try
            {
                memoryStream = new MemoryStream();
                int count;
                while ((count = await stream.ReadAsync(buffer).ConfigureAwait(false)) > 0)
                {
                    memoryStream.Write(buffer, 0, count);
                }

                return memoryStream.ToArray();
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

    }
}
