using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Wpf.Controls;
using System.Reflection;
using System.Windows.Controls;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class TradeItemAutoTemplate : RPGDataEditor.Wpf.Controls.TradeItemAutoTemplate
    {
        protected override StackPanel BuildPanel(PropertyInfo info)
        {
            StackPanel panel = base.BuildPanel(info);
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TradeItemModel.Nbt) });
            return panel;
        }
    }
}
