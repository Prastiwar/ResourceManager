using System;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public abstract class AutoTemplate
    {
        public AutoTemplate(Type type) => Type = type;

        protected Type Type { get; }

        public abstract DependencyObject LoadContent(PropertyInfo info);
    }

    public abstract class AutoTemplate<T> : AutoTemplate
    {
        public AutoTemplate() : base(typeof(T)) { }
    }
}
