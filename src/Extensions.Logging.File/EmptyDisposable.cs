using System;

namespace Extensions.Logging.File
{
    public class EmptyDisposable : IDisposable
    {
        public static IDisposable Instance { get; }

        static EmptyDisposable() => Instance = new EmptyDisposable();

        public void Dispose() { }
    }
}
