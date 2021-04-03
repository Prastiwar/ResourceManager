using System;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public abstract class ControlGenerateTemplate
    {
        public ControlGenerateTemplate(PropertyInfo info, Type type)
        {
            Info = info;
            Type = type;
        }

        protected Type Type { get; }

        protected PropertyInfo Info { get; }

        public abstract DependencyObject LoadContent();
    }

    public abstract class ControlGenerateTemplate<T> : ControlGenerateTemplate
    {
        public ControlGenerateTemplate(PropertyInfo info) : base(info, typeof(T)) { }
    }
}
