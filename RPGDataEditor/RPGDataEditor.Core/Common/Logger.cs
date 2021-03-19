using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Core
{
    public static class Logger
    {
        private static readonly string defaultFilePath = $"./logs_{DateTime.Now.ToString("dd_MM_yyyy")}.txt";

        public static string FilePath { get; set; } = defaultFilePath;

        public static string Template { get; set; } = $"[{DateTime.Now.ToString("HH:MM:ss")}]: {{0}}";

        public static void Error(string message, Exception exception = null)
        {
            Write(message);
            if (exception != null)
            {
                Write(exception.Message);
                Write(exception.StackTrace);
            }
        }

        public static void Log(string message) => Write(message);

        public static void LogTrace(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
            => Write(message + $" FilePath: {sourceFilePath}; Line: {sourceLineNumber}; Member: {memberName}");

        public static void ErrorFtp(FtpWebResponse response) => LogTrace($"FTP Error, Status code: {response.StatusCode}");

        private static string GetTemplatedMessage(string message) => string.Format(Template, message);

        private static void WriteRaw(string message) => System.IO.File.AppendAllText(FilePath, message + "\n");

        private static void Write(string message) => WriteRaw(GetTemplatedMessage(message));
    }
}
