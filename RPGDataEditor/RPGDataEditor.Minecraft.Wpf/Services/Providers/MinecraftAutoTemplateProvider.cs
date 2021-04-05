using Prism.Ioc;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Minecraft.Wpf.Controls;
using RPGDataEditor.Wpf.Providers;

namespace RPGDataEditor.Minecraft.Wpf.Providers
{
    public class MinecraftAutoTemplateProvider : AutoTemplateProvider
    {
        public MinecraftAutoTemplateProvider(IContainerProvider container) : base(container) { }

        public override void RegisterDefaults(IContainerRegistry containerRegistry)
        {
            base.RegisterDefaults(containerRegistry);
            RegisterAutoTemplate<EquipmentModel>(containerRegistry, new EquipmentModelAutoTemplate());
        }
    }
}
