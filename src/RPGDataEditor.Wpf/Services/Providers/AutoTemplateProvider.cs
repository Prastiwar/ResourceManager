using Prism.Ioc;
using RPGDataEditor.Models;
using RPGDataEditor.Wpf.Controls;
using RPGDataEditor.Wpf.Controls.AutoGeneration.Models;
using System;
using System.Collections.Generic;
using System.Security;
using System.Windows.Media;

namespace RPGDataEditor.Wpf.Providers
{
    public class AutoTemplateProvider : IAutoTemplateProvider
    {
        public AutoTemplateProvider(IContainerProvider container) => this.container = container;

        private readonly IContainerProvider container;

        public AutoTemplate Resolve(Type type)
        {
            try
            {
                return container.Resolve(typeof(AutoTemplate), type.FullName) as AutoTemplate;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual void RegisterDefaults(IContainerRegistry containerRegistry)
        {
            RegisterAutoTemplate<int>(containerRegistry, new IntAutoTemplate());
            RegisterAutoTemplate<double>(containerRegistry, new DoubleAutoTemplate());
            RegisterAutoTemplate<float>(containerRegistry, new FloatAutoTemplate());
            RegisterAutoTemplate<bool>(containerRegistry, new BoolAutoTemplate());

            RegisterAutoTemplate<string>(containerRegistry, new StringAutoTemplate());
            RegisterAutoTemplate<Position>(containerRegistry, new PositionAutoTemplate());
            RegisterAutoTemplate<Color>(containerRegistry, new ColorAutoTemplate());
            RegisterAutoTemplate<SecureString>(containerRegistry, new SecureStringAutoTemplate());

            RegisterAutoTemplate<DialogueOption>(containerRegistry, new DialogueOptionAutoTemplate());
            RegisterAutoTemplate<IQuestTask>(containerRegistry, new QuestTaskAutoTemplate());
            RegisterAutoTemplate<Requirement>(containerRegistry, new RequirementAutoTemplate());
            RegisterAutoTemplate<TalkLine>(containerRegistry, new TalkLineAutoTemplate());
            RegisterAutoTemplate<TradeItem>(containerRegistry, new TradeItemAutoTemplate());

            RegisterListDataAutoTemplate<Requirement>(containerRegistry);
            RegisterListDataAutoTemplate<DialogueOption>(containerRegistry);
            RegisterListDataAutoTemplate<IQuestTask>(containerRegistry);
            RegisterListDataAutoTemplate<TalkLine>(containerRegistry);
            RegisterListDataAutoTemplate<TradeItem>(containerRegistry);
        }

        protected void RegisterListDataAutoTemplate<T>(IContainerRegistry containerRegistry)
            => RegisterListDataAutoTemplate<T>(containerRegistry, new DataCollectionAutoTemplate<T>());

        protected void RegisterListDataAutoTemplate<T>(IContainerRegistry containerRegistry, AutoTemplate template)
            => containerRegistry.RegisterInstance<AutoTemplate>(template, typeof(IList<T>).FullName);

        protected void RegisterAutoTemplate<T>(IContainerRegistry containerRegistry, AutoTemplate template)
            => containerRegistry.RegisterInstance<AutoTemplate>(template, typeof(T).FullName);
    }
}
