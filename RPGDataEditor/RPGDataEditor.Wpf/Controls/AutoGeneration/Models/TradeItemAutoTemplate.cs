using MaterialDesignThemes.Wpf;
using RPGDataEditor.Core.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class TradeItemAutoTemplate : AutoTemplate<TradeItemModel>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            Card card = new Card() {
                Margin = new Thickness(5),
                Padding = new Thickness(5)
            };
            card.Content = BuildPanel(info);
            AutoControl.SetPreserveDataContext(card, false);
            return card;
        }

        protected virtual StackPanel BuildPanel(PropertyInfo info)
        {
            StackPanel panel = new StackPanel();
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TradeItemModel.Item) });
            StackPanel numericPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            numericPanel.Children.Add(new AutoControl() {
                PropertyName = nameof(TradeItemModel.Count),
                MinWidth = 128
            });
            numericPanel.Children.Add(new AutoControl() {
                PropertyName = nameof(TradeItemModel.Buy),
                MinWidth = 128
            });
            numericPanel.Children.Add(new AutoControl() {
                PropertyName = nameof(TradeItemModel.Sell),
                MinWidth = 128
            });
            panel.Children.Add(numericPanel);
            return panel;
        }
    }
}
