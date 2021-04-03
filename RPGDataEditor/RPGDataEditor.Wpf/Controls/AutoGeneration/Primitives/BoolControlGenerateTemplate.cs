using RPGDataEditor.Core;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class BoolControlGenerateTemplate : ControlGenerateTemplate<bool>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            CheckBox box = new CheckBox {
                Margin = new Thickness(5),
                Content = info.Name.MakeFriendlyName()
            };
            box.SetBinding(CheckBox.IsCheckedProperty, new Binding(info.Name));
            return box;
        }
    }
}
