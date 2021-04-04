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
        public static readonly DependencyProperty PreserveDataContextProperty =
            DependencyProperty.RegisterAttached("PreserveDataContext", typeof(bool), typeof(AttachProperties), new PropertyMetadata(true));
        public static void SetPreserveDataContext(UIElement element, bool value) => element.SetValue(PreserveDataContextProperty, value);
        public static bool GetPreserveDataContext(UIElement element) => (bool)element.GetValue(PreserveDataContextProperty);

        public static DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(AutoControl));
        public string PropertyName {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static DependencyProperty PropertyTypeProperty =
            DependencyProperty.Register(nameof(PropertyType), typeof(Type), typeof(AutoControl));
        public Type PropertyType {
            get => (Type)GetValue(PropertyTypeProperty);
            set => SetValue(PropertyTypeProperty, value);
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
            object context = DataContext;
            if (context == null || string.IsNullOrEmpty(PropertyName))
            {
                return null;
            }
            DependencyObject control = LoadControl(context, PropertyName);
            if (control == null)
            {
                return null;
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
            if (PropertyType != null)
            {
                type = PropertyType;
            }
            AutoTemplate template = provider.Resolve(type);
            while (template == null)
            {
                type = type.BaseType;
                template = provider.Resolve(type);
            }
            DependencyObject control = template?.LoadContent(property);
            if (control is FrameworkElement element)
            {
                if (GetPreserveDataContext(element))
                {
                    element.DataContext = DataContext;
                }
                else
                {
                    element.SetBinding(FrameworkElement.DataContextProperty, propertyName);
                }
                foreach (DependencyProperty prop in this.EnumerateAttachedProperties())
                {
                    element.SetValue(prop, GetValue(prop));
                }
            }
            return control;
        }
    }
}
