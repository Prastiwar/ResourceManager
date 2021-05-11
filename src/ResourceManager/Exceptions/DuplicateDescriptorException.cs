using System;
using System.Runtime.Serialization;

namespace ResourceManager
{
    [Serializable]
    public class DuplicateDescriptorException : Exception
    {
        public DuplicateDescriptorException() { }

        public DuplicateDescriptorException(string message) : base(message) { }

        public DuplicateDescriptorException(string message, Exception innerException) : base(message, innerException) { }

        protected DuplicateDescriptorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}