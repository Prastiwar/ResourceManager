using System;
using System.Runtime.Serialization;

namespace ResourceManager
{
    [Serializable]
    public class DescribtionException : Exception
    {
        public DescribtionException() { }

        public DescribtionException(string message) : base(message) { }

        public DescribtionException(string message, Exception innerException) : base(message, innerException) { }

        protected DescribtionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
