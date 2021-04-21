using RPGDataEditor.Services;
using System;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Core
{
    public class LocalFileLogger : ILogger
    {
        private static readonly string defaultFilePath = $"./logs_{DateTime.Now.ToString("dd_MM_yyyy")}.txt";

        public string FilePath { get; set; } = defaultFilePath;

        public string Template { get; set; } = $"[{DateTime.Now.ToString("HH:mm:ss")}]: {{0}}";

        public void Error(string message, Exception exception = null)
        {
            Write(message);
            if (exception != null)
            {
                Write(exception.Message);
                Write(exception.StackTrace);
            }
        }
        public void Warn(string message) => Log(message);

        public void Log(string message) => Write(message);

        public void Trace(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
            => Write(message + $" FilePath: {sourceFilePath}; Line: {sourceLineNumber}; Member: {memberName}");

        private string GetTemplatedMessage(string message) => string.Format(Template, message);

        private void WriteRaw(string message)
        {
            lock (FilePath)
            {
                System.IO.File.AppendAllText(FilePath, message + "\n");
            }
        }

        private void Write(string message) => WriteRaw(GetTemplatedMessage(message));
    }
}
