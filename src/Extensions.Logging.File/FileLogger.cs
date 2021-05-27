using Microsoft.Extensions.Logging;
using System;
using System.IO;

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
            string originalFileFullPath = Options.FilePathFunc.Invoke();
            string fileFullPath = originalFileFullPath;
            FileInfo logFile = new FileInfo(fileFullPath);
            int counter = 1;
            while (logFile.Exists && counter <= Options.MaxFilesCount)
            {
                bool exceedsMaxSize = logFile.Length >= Options.MaxFileSize;
                if (exceedsMaxSize)
                {
                    fileFullPath = $"{Path.GetFileNameWithoutExtension(logFile.Name)}({counter}){logFile.Extension}";
                    logFile = new FileInfo(fileFullPath);
                }
                else
                {
                    break;
                }
                counter++;
            }
            BuildLogMessageHandler messageBuilder = Options.LogMessageBuilder;
            if (Options.LogMessageBuilder == null)
            {
                messageBuilder = (m, c, l, e, ex) => m;
            }
            string formattedMessage = messageBuilder.Invoke(message, CategoryName, logLevel, eventId, exception) + Environment.NewLine;
            bool canAppend = logFile.Exists && logFile.Length >= Options.MaxFileSize;
            if (canAppend)
            {
                lock (locker)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileFullPath));
                    System.IO.File.AppendAllText(fileFullPath, formattedMessage);
                }
            }
            else
            {
                lock (locker)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(originalFileFullPath));
                    System.IO.File.WriteAllText(originalFileFullPath, formattedMessage);
                }
            }
        }
    }
}
