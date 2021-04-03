using RPGDataEditor.Core;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class BoolControlGenerateTemplate : ControlGenerateTemplate<bool>
    {
        public BoolControlGenerateTemplate(PropertyInfo info) : base(info) { }

        public override DependencyObject LoadContent()
        {
            CheckBox box = new CheckBox {
                Margin = new Thickness(5),
                Content = Info.Name.MakeFriendlyName()
            };
            box.SetBinding(FrameworkElement.DataContextProperty, new Binding("Model"));
            box.SetBinding(CheckBox.IsCheckedProperty, new Binding(Info.Name));
            return box;
        }
    }
}
