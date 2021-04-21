using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Wpf.Controls;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class EquipmentModelAutoTemplate : AutoTemplate<EquipmentModel>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            StackPanel panel = new StackPanel();
            panel.Children.Add(new AutoControl() { PropertyName = nameof(EquipmentModel.MainHand) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(EquipmentModel.OffHand) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(EquipmentModel.Head) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(EquipmentModel.Chest) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(EquipmentModel.Legs) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(EquipmentModel.Feet) });
            AutoControl.SetPreserveDataContext(panel, false);
            return panel;
        }
    }
}
