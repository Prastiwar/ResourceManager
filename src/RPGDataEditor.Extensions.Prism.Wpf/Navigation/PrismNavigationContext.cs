using Prism.Regions;
using RPGDataEditor.Mvvm.Navigation;
using System;

namespace RPGDataEditor.Extensions.Prism.Wpf.Navigation
{
    public class PrismNavigationContext : INavigationContext
    {
        public PrismNavigationContext(NavigationContext context) => Context = context;

        public NavigationContext Context { get; }

        public Uri Uri => Context.Uri;

        public NavigationParameters Parameters => Context.Parameters;
    }
}
