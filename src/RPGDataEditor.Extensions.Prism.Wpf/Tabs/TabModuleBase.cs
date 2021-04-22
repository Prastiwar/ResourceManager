using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace RPGDataEditor.Mvvm
{
    public abstract class RegionTabModuleBase : IModule
    {
        public RegionTabModuleBase(IRegionManager regionManager) => RegionManager = regionManager;

        protected IRegionManager RegionManager { get; }

        protected string TargetRegionName { get; set; } = "ContentRegion";

        protected abstract Type[] GetTabTypes();

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            foreach (Type tabType in GetTabTypes())
            {
                RegionManager.RegisterViewWithRegion(TargetRegionName, tabType);
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
