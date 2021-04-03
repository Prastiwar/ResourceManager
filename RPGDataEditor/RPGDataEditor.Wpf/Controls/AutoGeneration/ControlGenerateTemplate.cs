using System;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public abstract class ControlGenerateTemplate
    {
        public ControlGenerateTemplate(Type type) => Type = type;

        protected Type Type { get; }

        public abstract DependencyObject LoadContent(PropertyInfo info);
    }

    public abstract class ControlGenerateTemplate<T> : ControlGenerateTemplate
    {
        public ControlGenerateTemplate() : base(typeof(T)) { }
    }
}
