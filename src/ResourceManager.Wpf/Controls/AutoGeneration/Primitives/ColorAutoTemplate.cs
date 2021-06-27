using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ResourceManager.Wpf.Controls
{
    public class ColorAutoTemplate : AutoTemplate<Color>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            MaterialColorPicker picker = new MaterialColorPicker {
                Margin = new Thickness(5)
            };
            picker.SetBinding(MaterialColorPicker.ColorProperty, new Binding(options.BindingName) { Mode = BindingMode.TwoWay });
            return picker;
        }
    }
}
