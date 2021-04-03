using Prism.Ioc;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace RPGDataEditor.Wpf.Providers
{
    public class ControlGenerateTemplateProvider : IControlGenerateTemplateProvider
    {
        public ControlGenerateTemplateProvider(IContainerProvider container) => this.container = container;

        private readonly IContainerProvider container;

        public ControlGenerateTemplate Resolve(Type type)
        {
            try
            {
                return container.Resolve(typeof(ControlGenerateTemplate), type.FullName) as ControlGenerateTemplate;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void RegisterDefaults(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new StringControlGenerateTemplate(), typeof(string).FullName);
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new IntControlGenerateTemplate(), typeof(int).FullName);
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new DoubleControlGenerateTemplate(), typeof(double).FullName);
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new FloatControlGenerateTemplate(), typeof(float).FullName);
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new BoolControlGenerateTemplate(), typeof(bool).FullName);
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new PositionControlGenerateTemplate(), typeof(Position).FullName);
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new ColorControlGenerateTemplate(), typeof(Color).FullName);
            containerRegistry.RegisterInstance<ControlGenerateTemplate>(new RequirementDataCollectionControlGenerateTemplate(), typeof(IList<PlayerRequirementModel>).FullName);
        }
    }
}
