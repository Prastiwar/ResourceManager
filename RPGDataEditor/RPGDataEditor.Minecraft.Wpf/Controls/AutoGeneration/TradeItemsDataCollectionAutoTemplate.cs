﻿using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf;
using RPGDataEditor.Wpf.Controls;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class TradeItemsDataCollectionAutoTemplate : DataCollectionAutoTemplate<IList<TradeItemModel>>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            ListDataCard listCard = base.LoadContent(info) as ListDataCard;
            listCard.AddItemCommand = Commands.AddListItemCommand(() => listCard.ItemsSource, () => new Models.TradeItemModel());
            return listCard;
        }
    }
}
