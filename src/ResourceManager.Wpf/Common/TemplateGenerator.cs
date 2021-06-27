using System;
using System.Windows;
using System.Windows.Controls;

namespace ResourceManager.Wpf
{
    /// <summary> Class that helps the creation of control and data templates by using delegates </summary>
    public static class TemplateGenerator
    {
        private sealed class _TemplateGeneratorControl : ContentControl
        {
            internal static readonly DependencyProperty factoryProperty
                = DependencyProperty.Register("Factory", typeof(Func<object>), typeof(_TemplateGeneratorControl), new PropertyMetadata(null, _FactoryChanged));

            private static void _FactoryChanged(DependencyObject instance, DependencyPropertyChangedEventArgs args)
            {
                _TemplateGeneratorControl control = (_TemplateGeneratorControl)instance;
                Func<object> factory = (Func<object>)args.NewValue;
                control.Content = factory();
            }
        }

        /// <summary> Creates a data-template that uses the given delegate to create new instances </summary>
        public static DataTemplate CreateDataTemplate(Func<object> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(_TemplateGeneratorControl));
            frameworkElementFactory.SetValue(_TemplateGeneratorControl.factoryProperty, factory);

            DataTemplate dataTemplate = new DataTemplate(typeof(DependencyObject)) {
                VisualTree = frameworkElementFactory
            };
            return dataTemplate;
        }

        /// <summary> Creates a control-template that uses the given delegate to create new instances </summary>
        public static ControlTemplate CreateControlTemplate(Type controlType, Func<object> factory)
        {
            if (controlType == null)
            {
                throw new ArgumentNullException(nameof(controlType));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(_TemplateGeneratorControl));
            frameworkElementFactory.SetValue(_TemplateGeneratorControl.factoryProperty, factory);

            ControlTemplate controlTemplate = new ControlTemplate(controlType) {
                VisualTree = frameworkElementFactory
            };
            return controlTemplate;
        }
    }
}
