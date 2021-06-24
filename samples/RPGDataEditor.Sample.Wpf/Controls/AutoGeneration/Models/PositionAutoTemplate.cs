using RPGDataEditor.Sample.Models;
using RPGDataEditor.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Sample.Wpf.Controls
{
    public class PositionAutoTemplate : AutoTemplate<Position>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            PositionBox field = new PositionBox {
                Margin = new Thickness(5),
                Orientation = Orientation.Horizontal
            };
            field.SetBinding(PositionBox.PositionProperty, new Binding(options.BindingName) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            return field;
        }
    }
}
