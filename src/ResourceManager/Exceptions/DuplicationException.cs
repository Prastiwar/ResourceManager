using System;
using System.Runtime.Serialization;

namespace ResourceManager
{
    [Serializable]
    public class DuplicationException : Exception
    {
        public DuplicationException() { }

        public DuplicationException(string message) : base(message) { }

        public DuplicationException(string message, Exception innerException) : base(message, innerException) { }

        protected DuplicationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}