using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RPGDataEditor.Wpf.Controls
{
    public class ColorControlGenerateTemplate : ControlGenerateTemplate<Color>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            MaterialColorPicker picker = new MaterialColorPicker {
                Margin = new Thickness(5)
            };
            picker.SetBinding(MaterialColorPicker.ColorProperty, new Binding(info.Name) { Mode = BindingMode.TwoWay });
            return picker;
        }
    }
}
