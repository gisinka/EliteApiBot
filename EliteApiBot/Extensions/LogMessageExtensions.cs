using Discord;
using Vostok.Logging.Abstractions;

namespace EliteApiBot.Extensions;

internal static class LogMessageExtensions
{
    public static LogEvent Convert(this LogMessage logMessage)
    {
        return new LogEvent(logMessage.Severity.Convert(), DateTimeOffset.Now, logMessage.Message, null, logMessage.Exception);
    }
}