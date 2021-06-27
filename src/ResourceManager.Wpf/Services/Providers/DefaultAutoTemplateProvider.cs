using Prism.Ioc;
using ResourceManager.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Security;
using System.Windows.Media;

namespace ResourceManager.Wpf.Providers
{
    public class DefaultAutoTemplateProvider : IAutoTemplateProvider
    {
        public DefaultAutoTemplateProvider(IContainerProvider container) => this.container = container;

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
            RegisterAutoTemplate<float>(containerRegistry, new FloatAutoTemplate());
            RegisterAutoTemplate<double>(containerRegistry, new DoubleAutoTemplate());
            RegisterAutoTemplate<decimal>(containerRegistry, new DecimalAutoTemplate());
            RegisterAutoTemplate<bool>(containerRegistry, new BoolAutoTemplate());

            RegisterAutoTemplate<string>(containerRegistry, new StringAutoTemplate());
            RegisterAutoTemplate<object>(containerRegistry, new ObjectAutoTemplate());
            RegisterAutoTemplate<Color>(containerRegistry, new ColorAutoTemplate());
            RegisterAutoTemplate<SecureString>(containerRegistry, new SecureStringAutoTemplate());
        }

        protected void RegisterListDataAutoTemplate<T>(IContainerRegistry containerRegistry)
            => RegisterListDataAutoTemplate<T>(containerRegistry, new DataCollectionAutoTemplate<T>());

        protected void RegisterListDataAutoTemplate<T>(IContainerRegistry containerRegistry, AutoTemplate template)
            => containerRegistry.RegisterInstance<AutoTemplate>(template, typeof(IList<T>).FullName);

        protected void RegisterAutoTemplate<T>(IContainerRegistry containerRegistry, AutoTemplate template)
            => containerRegistry.RegisterInstance<AutoTemplate>(template, typeof(T).FullName);
    }
}
