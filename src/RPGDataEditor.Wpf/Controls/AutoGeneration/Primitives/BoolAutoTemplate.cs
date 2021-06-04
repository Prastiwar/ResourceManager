using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class BoolAutoTemplate : AutoTemplate<bool>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            CheckBox box = new CheckBox {
                Margin = new Thickness(5),
                Content = options.BindingName.MakeFriendlyName()
            };
            box.SetBinding(CheckBox.IsCheckedProperty, new Binding(options.BindingName));
            return box;
        }
    }
}
