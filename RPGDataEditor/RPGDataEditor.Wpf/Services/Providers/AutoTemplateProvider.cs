using Prism.Ioc;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf.Controls;
using RPGDataEditor.Wpf.Controls.AutoGeneration.Models;
using System;
using System.Collections.Generic;
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

            RegisterAutoTemplate<DialogueOptionModel>(containerRegistry, new DialogueOptionAutoTemplate());
            RegisterAutoTemplate<QuestTask>(containerRegistry, new QuestTaskAutoTemplate());
            RegisterAutoTemplate<PlayerRequirementModel>(containerRegistry, new RequirementAutoTemplate());

            RegisterListDataAutoTemplate<PlayerRequirementModel>(containerRegistry);
            RegisterListDataAutoTemplate<DialogueOptionModel>(containerRegistry);
            RegisterListDataAutoTemplate<QuestTask>(containerRegistry);
        }

        protected void RegisterListDataAutoTemplate<T>(IContainerRegistry containerRegistry)
            => containerRegistry.RegisterInstance<AutoTemplate>(new DataCollectionAutoTemplate<T>(), typeof(IList<T>).FullName);

        protected void RegisterAutoTemplate<T>(IContainerRegistry containerRegistry, AutoTemplate template)
            => containerRegistry.RegisterInstance<AutoTemplate>(template, typeof(T).FullName);
    }
}
