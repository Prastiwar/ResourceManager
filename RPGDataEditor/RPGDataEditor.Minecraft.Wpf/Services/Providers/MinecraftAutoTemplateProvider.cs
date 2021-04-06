using Prism.Ioc;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Minecraft.Wpf.Controls;
using RPGDataEditor.Wpf.Controls;
using RPGDataEditor.Wpf.Providers;
using System.Collections.Generic;

namespace RPGDataEditor.Minecraft.Wpf.Providers
{
    public class MinecraftAutoTemplateProvider : AutoTemplateProvider
    {
        public MinecraftAutoTemplateProvider(IContainerProvider container) : base(container) { }

        public override void RegisterDefaults(IContainerRegistry containerRegistry)
        {
            base.RegisterDefaults(containerRegistry);
            RegisterAutoTemplate<EquipmentModel>(containerRegistry, new EquipmentModelAutoTemplate());
            RegisterAutoTemplate<Core.Models.TradeItemModel>(containerRegistry, new Controls.TradeItemAutoTemplate());
            RegisterAutoTemplate<TradeItemModel>(containerRegistry, new Controls.TradeItemAutoTemplate());

            RegisterListDataAutoTemplate<TradeItemModel>(containerRegistry, new TradeItemsDataCollectionAutoTemplate());
            containerRegistry.RegisterInstance<AutoTemplate>(new TradeItemsDataCollectionAutoTemplate(), typeof(IList<Core.Models.TradeItemModel>).FullName);
        }
    }
}
