using FluentValidation;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Minecraft.Providers;
using RPGDataEditor.Minecraft.Validation;
using RPGDataEditor.Minecraft.Wpf.Providers;
using RPGDataEditor.Models;
using RPGDataEditor.Providers;
using RPGDataEditor.Wpf.Providers;
using System;
using System.Reflection;

namespace RPGDataEditor.Minecraft.Wpf
{
    public partial class App : RPGDataEditor.Wpf.RpgDataEditorApplication
    {
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) => {
                string viewName = viewType.FullName;
                string minecraftAssembly = typeof(App).Assembly.FullName;
                string viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                string shortViewModelName = viewName.Replace(".Views.", ".ViewRPGDataEditor.Models.");
                string shortMcViewModelName = shortViewModelName.Replace("RPGDataEditor.Wpf", "RPGDataEditor.Minecraft.Wpf");

                string minecraftViewModelName = $"{shortMcViewModelName}ViewModel, {minecraftAssembly}";
                string viewModelName = $"{shortViewModelName}ViewModel, {viewAssemblyName}";
                Type type = Type.GetType(minecraftViewModelName);
                return type ?? Type.GetType(viewModelName);
            });
        }

        protected override JsonSerializerSettings CreateJsonSettings()
        {
            PrettyOrderPropertyResolver propResolver = new PrettyOrderPropertyResolver();
            propResolver.SetAllLetterCase(Lettercase.CamelCase);
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = propResolver,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new NumberCastsConverter());
            settings.Converters.Add(new Serialization.PlayerRequirementJsonConverter());

            settings.Converters.Add(new Serialization.NpcJobJsonConverter());
            settings.Converters.Add(new Serialization.NpcJsonConverter());
            settings.Converters.Add(new Serialization.TradeItemJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new Serialization.QuestTaskJsonConverter());
            settings.Converters.Add(new QuestJsonConverter());

            settings.Converters.Add(new Serialization.DialogueJsonConverter());
            settings.Converters.Add(new Serialization.DialogueOptionJsonConverter());
            settings.Converters.Add(new Serialization.TalkDataJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());

            settings.Converters.Add(new ConnectionConfigJsonConverter());
            settings.Converters.Add(new ConnectionSettingsJsonConverter());
            return settings;
        }

        protected override void RegisterProviders(IContainerRegistry containerRegistry)
        {
            base.RegisterProviders(containerRegistry);
            containerRegistry.RegisterSingleton(typeof(IImplementationProvider<>), typeof(McImplementationProvider<>));
            AutoTemplateProvider controlProvider = new MinecraftAutoTemplateProvider(Container);
            controlProvider.RegisterDefaults(containerRegistry);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(controlProvider);
        }

        protected override void RegisterValidators(IContainerRegistry containerRegistry)
        {
            base.RegisterValidators(containerRegistry);
            containerRegistry.Register<IValidator<RPGDataEditor.Models.Npc>, NpcValidator>();
            containerRegistry.Register<IValidator<Quest>, QuestValidator>();
            containerRegistry.Register<IValidator<RPGDataEditor.Models.Dialogue>, DialogueValidator>();
            containerRegistry.Register<IValidator<Equipment>, EquipmentValidator>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();
    }
}
