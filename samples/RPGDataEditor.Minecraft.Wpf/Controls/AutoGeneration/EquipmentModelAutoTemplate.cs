using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class EquipmentAutoTemplate : AutoTemplate<Equipment>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            StackPanel panel = new StackPanel();
            panel.Children.Add(new AutoControl() { PropertyName = nameof(Equipment.MainHand) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(Equipment.OffHand) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(Equipment.Head) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(Equipment.Chest) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(Equipment.Legs) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(Equipment.Feet) });
            AutoControl.SetPreserveDataContext(panel, false);
            return panel;
        }
    }
}
