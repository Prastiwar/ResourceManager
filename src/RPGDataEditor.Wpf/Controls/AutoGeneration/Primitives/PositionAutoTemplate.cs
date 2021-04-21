using RPGDataEditor.Core.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class PositionAutoTemplate : AutoTemplate<Position>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            BlockPosField field = new BlockPosField {
                Margin = new Thickness(5),
                Orientation = Orientation.Horizontal
            };
            field.SetBinding(BlockPosField.PositionProperty, new Binding(info.Name) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            return field;
        }
    }
}
