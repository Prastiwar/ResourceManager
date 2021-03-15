using System;

namespace RPGDataEditor.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotValidableAttribute : Attribute { }
}
