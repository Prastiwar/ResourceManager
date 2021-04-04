using RPGDataEditor.Core;
using RPGDataEditor.Wpf.Providers;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace RPGDataEditor.Wpf.Controls
{
    [ContentProperty(nameof(PropertyOverrides))]
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

        private SetterBaseCollection propertyOverrides;
        public SetterBaseCollection PropertyOverrides {
            get {
                VerifyAccess();
                return propertyOverrides ??= new SetterBaseCollection();
            }
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
            return control;
        }

        protected virtual DependencyObject LoadControl(object context, string propertyName)
        {
            Type type = null;
            PropertyInfo property = null;
            if (propertyName == ".")
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
            IAutoTemplateProvider provider = Application.Current.TryResolve<IAutoTemplateProvider>();
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
                if (PropertyOverrides != null)
                {
                    foreach (Setter setter in PropertyOverrides)
                    {
                        element.SetValue(setter.Property, setter.Value);
                    }
                }
            }
            return control;
        }
    }
}
