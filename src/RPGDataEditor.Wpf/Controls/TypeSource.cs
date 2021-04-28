using System;

namespace RPGDataEditor.Wpf.Controls
{
    public class TypeSource
    {
        public TypeSource(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
