using Prism.Regions;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Mvvm
{
    public class TabControlAdapter : RegionAdapterBase<TabControl>
    {
        public TabControlAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory) { }

        private TabControl regionTarget;

        private bool cancelSelection;

        private void OnTabsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (UserControl item in e.NewItems)
                {
                    regionTarget.Items.Add(CreateTabItem(item));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (UserControl item in e.OldItems)
                {
                    TabItem tabTodelete = regionTarget.Items.OfType<TabItem>().FirstOrDefault(n => n.Content == item);
                    regionTarget.Items.Remove(tabTodelete);
                }
            }
        }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            this.regionTarget = regionTarget;
            region.Views.CollectionChanged += OnTabsChanged;
            regionTarget.SelectionChanged += async (s, e) => {
                if(cancelSelection)
                {
                    return;
                }
                if (e.AddedItems.Count == 0)
                {
                    return;
                }
                TabItem addedTab = e.AddedItems[0] as TabItem;
                TabItem removedTab = e.RemovedItems.Count > 0 ? e.RemovedItems[0] as TabItem : null;
                if (addedTab == null)
                {
                    return;
                }
                ITabSwitchAsyncAware removedTabAware = GetSwitchAsyncAware(removedTab);
                ITabSwitchAsyncAware addedTabAware = GetSwitchAsyncAware(addedTab);
                string toUri = "navigation://" + addedTab.Header;
                NavigationContext context = new NavigationContext(region.NavigationService, new System.Uri(toUri));
                bool canNavigate = true;
                canNavigate = removedTabAware == null || await removedTabAware.CanSwitchFrom(context);
                canNavigate = canNavigate && (addedTabAware == null || await addedTabAware.CanSwitchTo(context));
                if (!canNavigate)
                {
                    int previousTabIndex = e.RemovedItems.Count > 0 ? regionTarget.Items.IndexOf(e.RemovedItems[0]) : -1;
                    cancelSelection = true;
                    regionTarget.SelectedIndex = previousTabIndex;
                    cancelSelection = false;
                    return;
                }
                await Notifier.CallAsync<ITabSwitchAsyncAware>(removedTab?.DataContext, aware => aware.OnNavigatedFromAsync(context));
                await Notifier.CallAsync<ITabSwitchAsyncAware>(removedTab?.Content, aware => aware.OnNavigatedFromAsync(context));
                await Notifier.CallAsync<ITabSwitchAsyncAware>(addedTab.DataContext, aware => aware.OnNavigatedToAsync(context));
                await Notifier.CallAsync<ITabSwitchAsyncAware>(addedTab.Content, aware => aware.OnNavigatedToAsync(context));
            };
        }

        private ITabSwitchAsyncAware GetSwitchAsyncAware(TabItem item) => item?.Content is FrameworkElement el ? el.DataContext as ITabSwitchAsyncAware : null;

        protected TabItem CreateTabItem(UserControl control)
        {
            TabItem item = new TabItem {
                Header = control.Name,
                Content = control,
                Width = 100,
                Height = 48,
                Padding = new System.Windows.Thickness(2)
            };
            item.SetResourceReference(TabItem.StyleProperty, "MaterialDesignNavigationRailTabItem");
            return item;
        }

        protected override IRegion CreateRegion() => new SingleActiveRegion();
    }
}
