using System;

namespace ResourceManager
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotValidableAttribute : Attribute { }
}
