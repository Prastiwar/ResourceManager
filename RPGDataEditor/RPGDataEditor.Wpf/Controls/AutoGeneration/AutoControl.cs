using RPGDataEditor.Core;
using RPGDataEditor.Wpf.Providers;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace RPGDataEditor.Wpf.Controls
{
    [ContentProperty(nameof(Root))]
    public class AutoControl : UserControl
    {
        public static DependencyProperty PropertyContextProperty =
            DependencyProperty.Register(nameof(PropertyContext), typeof(object), typeof(AutoControl));
        public object PropertyContext {
            get => GetValue(PropertyContextProperty);
            set => SetValue(PropertyContextProperty, value);
        }

        public static DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(AutoControl));
        public string PropertyName {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static DependencyProperty RootProperty =
            DependencyProperty.Register(nameof(Root), typeof(object), typeof(AutoControl));
        public object Root {
            get => GetValue(RootProperty);
            set => SetValue(RootProperty, value);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (Content == null)
            {
                Recreate();
            }
            this.AddValueChanged(PropertyContextProperty, OnContextChanged);
            this.AddValueChanged(PropertyNameProperty, OnContextChanged);
            DataContextChanged += OnDataContextChanged;
        }

        private void OnContextChanged(object sender, EventArgs e) => Recreate();

        protected virtual void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => Recreate();

        protected virtual void Recreate()
        {
            ClearContent();
            Content = CreateContent();
        }

        protected virtual void ClearContent() => Content = null;

        protected virtual DependencyObject CreateContent()
        {
            object context = GetBindingExpression(PropertyContextProperty) != null ? PropertyContext : PropertyContext ?? DataContext;
            if (context == null || string.IsNullOrEmpty(PropertyName))
            {
                return null;
            }
            DependencyObject control = LoadControl(context, PropertyName);
            if (control == null)
            {
                return null;
            }
            if (control is FrameworkElement element)
            {
                element.DataContext = context;
            }
            if (Root is Panel panel)
            {
                panel.Children.Add((UIElement)control);
                return panel;
            }
            return control;
        }

        protected virtual DependencyObject LoadControl(object context, string propertyName)
        {
            IAutoTemplateProvider provider = Application.Current.TryResolve<IAutoTemplateProvider>();
            Type type = null;
            PropertyInfo property = null;
            if (PropertyName == ".")
            {
                type = context.GetType();
            }
            else
            {
                property = context.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    return null;
                }
                type = property.PropertyType;
            }
            AutoTemplate template = provider.Resolve(type);
            while (template == null)
            {
                type = type.BaseType;
                template = provider.Resolve(type);
            }
            return template?.LoadContent(property);
        }
    }
}
