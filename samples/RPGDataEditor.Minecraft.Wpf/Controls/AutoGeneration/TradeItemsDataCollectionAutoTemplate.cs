using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Wpf.Controls;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class TradeItemsDataCollectionAutoTemplate : DataCollectionAutoTemplate<IList<TradeItem>>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            ListDataCard listCard = base.LoadContent(info) as ListDataCard;
            listCard.AddItemCommand = RPGDataEditor.Wpf.Commands.AddListItemCommand(() => listCard.ItemsSource, () => new TradeItem());
            return listCard;
        }
    }
}
