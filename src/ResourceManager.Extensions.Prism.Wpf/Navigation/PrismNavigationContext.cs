using Prism.Regions;
using ResourceManager.Mvvm.Navigation;
using System;

namespace ResourceManager.Extensions.Prism.Wpf.Navigation
{
    public class PrismNavigationContext : INavigationContext
    {
        public PrismNavigationContext(NavigationContext context) => Context = context;

        public NavigationContext Context { get; }

        public Uri Uri => Context?.Uri;

        public NavigationParameters Parameters => Context?.Parameters;
    }
}
