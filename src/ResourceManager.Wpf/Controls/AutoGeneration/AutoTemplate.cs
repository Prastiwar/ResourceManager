using System;
using System.Windows;

namespace ResourceManager.Wpf.Controls
{
    public abstract class AutoTemplate
    {
        public AutoTemplate(Type type) => Type = type;

        protected Type Type { get; }

        public DependencyObject LoadContent(object context, string propertyName) => LoadContent(context, new TemplateOptions() { BindingName = propertyName });

        public abstract DependencyObject LoadContent(object context, TemplateOptions options);
    }

    public abstract class AutoTemplate<T> : AutoTemplate
    {
        public AutoTemplate() : base(typeof(T)) { }
    }
}
