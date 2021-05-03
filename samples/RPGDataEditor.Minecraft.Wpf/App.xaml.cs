using FluentValidation;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using RPGDataEditor.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Minecraft.Providers;
using RPGDataEditor.Minecraft.Serialization;
using RPGDataEditor.Minecraft.Validation;
using RPGDataEditor.Minecraft.Wpf.Providers;
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
            propResolver.IgnoreProperty(typeof(IdentifiableData), nameof(IdentifiableData.RepresentableString));
            propResolver.IgnoreProperty(typeof(ISessionContext), nameof(ISessionContext.ClientProvider));
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = propResolver,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new NumberCastsConverter());
            settings.Converters.Add(new Serialization.PlayerRequirementJsonConverter());

            settings.Converters.Add(new Serialization.NpcJobJsonConverter());
            settings.Converters.Add(new EquipmentJsonConverter());
            settings.Converters.Add(new Serialization.NpcJsonConverter());
            settings.Converters.Add(new Serialization.TradeItemJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new Serialization.QuestTaskJsonConverter());
            settings.Converters.Add(new QuestDataJsonConverter());

            settings.Converters.Add(new Serialization.DialogueJsonConverter());
            settings.Converters.Add(new Serialization.DialogueOptionJsonConverter());
            settings.Converters.Add(new Serialization.TalkDataJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());

            settings.Converters.Add(new ResourceClientJsonConverter());
            settings.Converters.Add(new OptionsDataJsonConverter());
            settings.Converters.Add(new SessionContextJsonConverter());
            if (Session is DefaultSessionContext context)
            {
                settings.Formatting = context.Options.PrettyPrint ? Formatting.Indented : Formatting.None;
            }
            return settings;
        }

        protected override void RegisterProviders(IContainerRegistry containerRegistry)
        {
            base.RegisterProviders(containerRegistry);
            containerRegistry.RegisterInstance<IModelProvider<Requirement>>(new RequirementProvider());
            containerRegistry.RegisterInstance<IModelProvider<QuestTask>>(new QuestTaskProvider());
            containerRegistry.Register(typeof(IModelProvider<>), typeof(McModelProvider<>));
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
