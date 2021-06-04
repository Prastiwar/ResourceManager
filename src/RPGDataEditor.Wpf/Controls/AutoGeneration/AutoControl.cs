using ResourceManager.Data;
using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Wpf.Providers;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        private static readonly char[] indexBraces = new char[] { '[', ']' };

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
            return LoadControl(context, PropertyName);
        }

        protected virtual DependencyObject LoadControl(object context, string propertyName)
        {
            Type propertyType = GetPropertyType(context, propertyName);
            AutoTemplate template = GetTemplate(propertyName, propertyType);
            DependencyObject control = template?.LoadContent(context, new TemplateOptions() { BindingName = propertyName });
            if (control is FrameworkElement element)
            {
                if (GetPreserveDataContext(element))
                {
                    element.SetBinding(FrameworkElement.DataContextProperty, new Binding(nameof(DataContext)) { Source = this });
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

        protected virtual Type GetPropertyType(object context, string propertyName)
        {
            if (PropertyType != null)
            {
                return PropertyType;
            }
            Type type = null;
            if (propertyName.StartsWith('[') && propertyName.EndsWith(']'))
            {
                if (context is IResource resource)
                {
                    ResourceProperty prop = resource.GetProperty(propertyName.Trim(indexBraces));
                    type = prop?.DeclaredType;
                }
                else
                {
                    PropertyInfo property = context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(prop => prop.GetIndexParameters().Length > 0);
                    if (property == null)
                    {
                        return null;
                    }
                    ParameterInfo[] indexParameters = property.GetIndexParameters();
                    if (indexParameters.Length != 1 || indexParameters[0].ParameterType != typeof(string))
                    {
                        return null;
                    }
                    type = property.PropertyType;
                }
            }
            else if (propertyName == ".")
            {
                type = context.GetType();
            }
            else
            {
                PropertyInfo property = context.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                type = property?.PropertyType;
            }
            return type;
        }

        protected virtual AutoTemplate GetTemplate(string propertyName, Type propertyType)
        {
            if (propertyType == null)
            {
                throw new InvalidOperationException("Cannot create control for unknown type for property name " + propertyName);
            }
            IAutoTemplateProvider provider = Application.Current.TryResolve<IAutoTemplateProvider>();
            AutoTemplate template = provider.Resolve(propertyType);
            while (template == null)
            {
                foreach (Type interfaceType in propertyType.GetInterfaces())
                {
                    template = provider.Resolve(interfaceType);
                    if (template != null)
                    {
                        propertyType = interfaceType;
                        break;
                    }
                }
                if (template != null)
                {
                    break;
                }
                propertyType = propertyType.BaseType;
                if (propertyType == null)
                {
                    break;
                }
                template = provider.Resolve(propertyType);
            }
            return template;
        }
    }
}
