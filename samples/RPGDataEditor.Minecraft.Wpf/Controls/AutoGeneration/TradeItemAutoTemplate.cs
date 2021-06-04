using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Wpf.Controls;
using System.Windows.Controls;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class TradeItemAutoTemplate : RPGDataEditor.Wpf.Controls.TradeItemAutoTemplate
    {
        protected override StackPanel BuildPanel(object context)
        {
            StackPanel panel = base.BuildPanel(context);
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TradeItem.Nbt) });
            return panel;
        }
    }
}
