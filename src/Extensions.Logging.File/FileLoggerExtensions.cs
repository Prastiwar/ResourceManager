using Extensions.Logging.File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath) => builder.AddFile(options => options.FilePathFunc = () => filePath);

        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Func<string> filePath) => builder.AddFile(options => options.FilePathFunc = filePath);

        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
        {
            FileLoggerOptions options = new FileLoggerOptions() {
                FilePathFunc = () => "log.txt",
                MaxFileSize = 2000000,
                MaxFilesCount = 1,
                LogMessageBuilder = BuildLogMessage
            };
            configure.Invoke(options);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(new FileLoggerProvider(options)));
            return builder;
        }

        private static string BuildLogMessage(string message, string categoryName, LogLevel logLevel, EventId eventId, Exception exception) => exception != null
                ? $"[{logLevel}] [{eventId}] [{categoryName}]: {message}{Environment.NewLine} Error Message: {exception.Message}{Environment.NewLine} {exception.StackTrace}"
                : $"[{logLevel}] [{eventId}] [{categoryName}]: {message}";
    }
}
