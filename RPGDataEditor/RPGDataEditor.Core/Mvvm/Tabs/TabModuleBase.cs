using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using RPGDataEditor.Core;
using System;

namespace RPGDataEditor.Wpf
{
    public abstract class RegionTabModuleBase : IModule
    {
        public RegionTabModuleBase(IRegionManager regionManager) => RegionManager = regionManager;

        protected IRegionManager RegionManager { get; }

        protected abstract Type[] GetTabTypes();

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            foreach (Type tabType in GetTabTypes())
            {
                RegionManager.RegisterViewWithRegion(RegionNames.ContentRegion, tabType);
            }
        }

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterNavigationTabs(containerRegistry);
            RegisterDialogs(containerRegistry);
        }

        protected virtual void RegisterNavigationTabs(IContainerRegistry containerRegistry)
        {
            foreach (Type tabType in GetTabTypes())
            {
                containerRegistry.RegisterForNavigation(tabType, tabType.Name);
            }
        }

        protected virtual void RegisterDialogs(IContainerRegistry containerRegistry) { }

    }
}
