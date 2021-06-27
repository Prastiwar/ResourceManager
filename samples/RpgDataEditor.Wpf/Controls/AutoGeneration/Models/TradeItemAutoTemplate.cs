using MaterialDesignThemes.Wpf;
using RpgDataEditor.Models;
using ResourceManager.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace RpgDataEditor.Wpf.Controls
{
    public class TradeItemAutoTemplate : AutoTemplate<TradeItem>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            Card card = new Card() {
                Margin = new Thickness(5),
                Padding = new Thickness(5)
            };
            card.Content = BuildPanel(context);
            AutoControl.SetPreserveDataContext(card, false);
            return card;
        }

        protected virtual StackPanel BuildPanel(object context)
        {
            StackPanel panel = new StackPanel();
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TradeItem.ItemId) });
            StackPanel numericPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            numericPanel.Children.Add(new AutoControl() {
                PropertyName = nameof(TradeItem.Count),
                MinWidth = 128
            });
            numericPanel.Children.Add(new AutoControl() {
                PropertyName = nameof(TradeItem.Buy),
                MinWidth = 128
            });
            numericPanel.Children.Add(new AutoControl() {
                PropertyName = nameof(TradeItem.Sell),
                MinWidth = 128
            });
            panel.Children.Add(numericPanel);
            return panel;
        }
    }
}
