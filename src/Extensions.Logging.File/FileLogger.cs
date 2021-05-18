using Microsoft.Extensions.Logging;
using System;

namespace Extensions.Logging.File
{
    public class FileLogger : ILogger
    {
        public FileLogger(string categoryName, FileLoggerOptions options)
        {
            CategoryName = categoryName;
            Options = options;
        }

        private readonly object locker = new object();

        protected string CategoryName { get; }

        protected FileLoggerOptions Options { get; }

        public IDisposable BeginScope<TState>(TState state) => EmptyDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = formatter.Invoke(state, exception);
            string level = logLevel.ToString();
            string fileFullPath = Options.FilePathFunc.Invoke();
            // TODO: Format message
            // TODO: Roll files
            lock (locker)
            {
                System.IO.File.AppendAllText(fileFullPath, message + "\n");
            }
        }
    }
}
