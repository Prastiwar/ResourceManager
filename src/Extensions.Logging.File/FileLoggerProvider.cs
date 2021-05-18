using Microsoft.Extensions.Logging;

namespace Extensions.Logging.File
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public FileLoggerProvider(FileLoggerOptions options) => Options = options;

        protected FileLoggerOptions Options { get; }

        public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, Options);

        public void Dispose() { }
    }
}
