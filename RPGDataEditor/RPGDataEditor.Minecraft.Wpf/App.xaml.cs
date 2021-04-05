using FluentValidation;
using Newtonsoft.Json;
using Prism.Ioc;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Minecraft.Providers;
using RPGDataEditor.Minecraft.Serialization;
using RPGDataEditor.Minecraft.Validation;
using RPGDataEditor.Minecraft.Wpf.Providers;
using RPGDataEditor.Wpf.Providers;

namespace RPGDataEditor.Minecraft.Wpf
{
    public partial class App : RPGDataEditor.Wpf.RpgDataEditorApplication
    {
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
            settings.Converters.Add(new EquipmentModelJsonConverter());
            settings.Converters.Add(new Serialization.NpcDataModelJsonConverter());
            settings.Converters.Add(new Serialization.TradeItemModelJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new Serialization.QuestTaskJsonConverter());
            settings.Converters.Add(new QuestDataJsonConverter());

            settings.Converters.Add(new Serialization.DialogueModelJsonConverter());
            settings.Converters.Add(new Serialization.DialogueOptionModelJsonConverter());
            settings.Converters.Add(new Serialization.TalkDataModelJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());


            settings.Converters.Add(new ResourceClientJsonConverter());
            settings.Converters.Add(new OptionsDataJsonConverter());
            settings.Converters.Add(new SessionContextJsonConverter());
            return settings;
        }

        protected override void RegisterProviders(IContainerRegistry containerRegistry)
        {
            base.RegisterProviders(containerRegistry);
            containerRegistry.RegisterInstance<IModelProvider<PlayerRequirementModel>>(new RequirementProvider());
            containerRegistry.RegisterInstance<IModelProvider<QuestTask>>(new QuestTaskProvider());
            containerRegistry.Register(typeof(IModelProvider<>), typeof(McModelProvider<>));
            AutoTemplateProvider controlProvider = new MinecraftAutoTemplateProvider(Container);
            controlProvider.RegisterDefaults(containerRegistry);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(controlProvider);
        }

        protected override void RegisterValidators(IContainerRegistry containerRegistry)
        {
            base.RegisterValidators(containerRegistry);
            containerRegistry.Register<IValidator<Core.Models.NpcDataModel>, NpcDataModelValidator>();
            containerRegistry.Register<IValidator<QuestModel>, QuestModelValidator>();
            containerRegistry.Register<IValidator<Core.Models.DialogueModel>, DialogueModelValidator>();
            containerRegistry.Register<IValidator<EquipmentModel>, EquipmentModelValidator>();
        }
    }
}
