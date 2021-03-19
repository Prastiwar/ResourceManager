using FluentValidation;
using Newtonsoft.Json;
using Prism.Ioc;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Minecraft.Wpf
{
    public partial class App : RPGDataEditorApp
    {
        protected override JsonSerializerSettings CreateJsonSettings()
        {
            PrettyOrderPropertyResolver propResolver = new PrettyOrderPropertyResolver();
            propResolver.SetAllLetterCase(Lettercase.CamelCase);
            propResolver.IgnoreProperty(typeof(IdentifiableData), nameof(IdentifiableData.RepresentableString));
            propResolver.IgnoreProperty(typeof(SessionContext), nameof(SessionContext.ConnectionProvider));
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = propResolver,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new NumberCastsConverter());
            settings.Converters.Add(new Serialization.PlayerRequirementJsonConverter());

            settings.Converters.Add(new Serialization.NpcJobJsonConverter());
            settings.Converters.Add(new Serialization.EquipmentModelJsonConverter());
            settings.Converters.Add(new Serialization.NpcDataModelJsonConverter());
            settings.Converters.Add(new Serialization.TradeItemModelJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new Serialization.QuestTaskJsonConverter());
            settings.Converters.Add(new QuestDataJsonConverter());

            settings.Converters.Add(new Serialization.DialogueModelJsonConverter());
            settings.Converters.Add(new Serialization.DialogueOptionModelJsonConverter());
            settings.Converters.Add(new Serialization.TalkDataModelJsonConverter());
            settings.Converters.Add(new Serialization.TalkLineJsonConverter());
            
            settings.Converters.Add(new ConnectionJsonConverter());
            return settings;
        }

        protected override void RegisterProviders(IContainerRegistry containerRegistry)
        {
            base.RegisterProviders(containerRegistry);
            containerRegistry.RegisterInstance<IRequirementProvider>(new McRequirementProvider());
        }

        protected override void RegisterValidators(IContainerRegistry containerRegistry)
        {
            base.RegisterValidators(containerRegistry);
            containerRegistry.Register<IValidator<Models.NpcDataModel>, Validation.NpcDataModelValidator>();
            containerRegistry.Register<IValidator<QuestModel>, Validation.QuestModelValidator>();
            containerRegistry.Register<IValidator<Models.DialogueModel>, Validation.DialogueModelValidator>();
        }
    }
}
