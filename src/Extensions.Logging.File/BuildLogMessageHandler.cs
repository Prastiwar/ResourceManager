using Microsoft.Extensions.Logging;
using System;

namespace Extensions.Logging.File
{
    public delegate string BuildLogMessageHandler(string message, string categoryName, LogLevel logLevel, EventId eventId, Exception exception);
}
