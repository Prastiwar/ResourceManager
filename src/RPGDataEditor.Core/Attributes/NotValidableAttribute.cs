using System;

namespace RPGDataEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotValidableAttribute : Attribute { }
}
