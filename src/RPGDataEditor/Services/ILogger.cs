using System;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Services
{
    public interface ILogger
    {
        void Error(string message, Exception exception = null);

        void Warn(string message);

        void Log(string message);

        void Trace(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
    }
}
