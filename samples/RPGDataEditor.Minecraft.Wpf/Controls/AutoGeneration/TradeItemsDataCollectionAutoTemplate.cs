using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Wpf.Controls;
using System.Collections.Generic;
using System.Windows;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class TradeItemsDataCollectionAutoTemplate : DataCollectionAutoTemplate<IList<TradeItem>>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            ListDataCard listCard = base.LoadContent(context, options) as ListDataCard;
            listCard.AddItemCommand = RPGDataEditor.Wpf.Commands.AddListItemCommand(() => listCard.ItemsSource, () => new TradeItem());
            return listCard;
        }
    }
}
