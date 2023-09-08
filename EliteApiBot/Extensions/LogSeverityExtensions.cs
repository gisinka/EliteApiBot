using Discord;
using LogLevel = Vostok.Logging.Abstractions.LogLevel;

namespace EliteApiBot.Extensions
{
    internal static class LogSeverityExtensions
    {
        public static LogLevel Convert(this LogSeverity logSeverity)
        {
            return logSeverity switch
            {
                LogSeverity.Critical => LogLevel.Fatal,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warn,
                LogSeverity.Info => LogLevel.Info,
                LogSeverity.Verbose => LogLevel.Debug,
                LogSeverity.Debug => LogLevel.Debug,
                _ => throw new ArgumentOutOfRangeException(nameof(logSeverity), logSeverity, null)
            };
        }
    }
}
