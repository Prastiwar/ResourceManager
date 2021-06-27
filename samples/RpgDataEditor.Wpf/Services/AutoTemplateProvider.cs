using Prism.Ioc;
using RpgDataEditor.Models;
using RpgDataEditor.Wpf.Controls;
using ResourceManager.Wpf.Providers;

namespace RpgDataEditor.Wpf.Providers
{
    public class AutoTemplateProvider : DefaultAutoTemplateProvider
    {
        public AutoTemplateProvider(IContainerProvider container) : base(container) { }

        public override void RegisterDefaults(IContainerRegistry containerRegistry)
        {
            base.RegisterDefaults(containerRegistry);
            RegisterAutoTemplate<Position>(containerRegistry, new PositionAutoTemplate());

            RegisterAutoTemplate<DialogueOption>(containerRegistry, new DialogueOptionAutoTemplate());
            RegisterAutoTemplate<IQuestTask>(containerRegistry, new QuestTaskAutoTemplate());
            RegisterAutoTemplate<Requirement>(containerRegistry, new RequirementAutoTemplate());
            RegisterAutoTemplate<TalkLine>(containerRegistry, new TalkLineAutoTemplate());
            RegisterAutoTemplate<TradeItem>(containerRegistry, new TradeItemAutoTemplate());
            RegisterAutoTemplate<NpcJob>(containerRegistry, new NpcJobAutoTemplate());

            RegisterListDataAutoTemplate<Requirement>(containerRegistry);
            RegisterListDataAutoTemplate<DialogueOption>(containerRegistry);
            RegisterListDataAutoTemplate<IQuestTask>(containerRegistry);
            RegisterListDataAutoTemplate<TalkLine>(containerRegistry);
            RegisterListDataAutoTemplate<TradeItem>(containerRegistry);
        }
    }
}
