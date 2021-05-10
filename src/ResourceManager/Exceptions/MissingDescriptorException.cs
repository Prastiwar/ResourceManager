using System;
using System.Runtime.Serialization;

namespace ResourceManager
{
    [Serializable]
    public class MissingDescriptorException : Exception
    {
        public MissingDescriptorException() { }

        public MissingDescriptorException(string message) : base(message) { }

        public MissingDescriptorException(string message, Exception innerException) : base(message, innerException) { }

        protected MissingDescriptorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
