using Prism.Regions;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Mvvm
{
    public class TabControlAdapter : RegionAdapterBase<TabControl>
    {
        public TabControlAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory) { }

        protected TabControl RegionTarget { get; private set; }

        protected bool CancelSelection { get; set; }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            RegionTarget = regionTarget;
            region.Views.CollectionChanged += OnTabsChanged;
            regionTarget.SelectionChanged += async (s, e) => {
                if (CancelSelection)
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
                AttachProperties.SetIsLoading(regionTarget, true);
                string toUri = "navigation://" + addedTab.Header;
                NavigationContext context = new NavigationContext(region.NavigationService, new System.Uri(toUri));
                bool canNavigate = await CanNavigateAsync(removedTab, addedTab, context);
                if (!canNavigate)
                {
                    int previousTabIndex = e.RemovedItems.Count > 0 ? regionTarget.Items.IndexOf(e.RemovedItems[0]) : -1;
                    CancelSelection = true;
                    regionTarget.SelectedIndex = previousTabIndex;
                    CancelSelection = false;
                    AttachProperties.SetIsLoading(regionTarget, false);
                    return;
                }
                await Notifier.CallAsync<ITabSwitchAsyncAware>(removedTab?.DataContext, aware => aware.OnNavigatedFromAsync(context));
                await Notifier.CallAsync<ITabSwitchAsyncAware>(removedTab?.Content, aware => aware.OnNavigatedFromAsync(context));
                await Notifier.CallAsync<ITabSwitchAsyncAware>(addedTab.DataContext, aware => aware.OnNavigatedToAsync(context));
                await Notifier.CallAsync<ITabSwitchAsyncAware>(addedTab.Content, aware => aware.OnNavigatedToAsync(context));
                AttachProperties.SetIsLoading(regionTarget, false);
            };
        }

        protected virtual void OnTabsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (UserControl item in e.NewItems)
                {
                    RegionTarget.Items.Add(CreateTabItem(item));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (UserControl item in e.OldItems)
                {
                    TabItem tabTodelete = RegionTarget.Items.OfType<TabItem>().FirstOrDefault(n => n.Content == item);
                    RegionTarget.Items.Remove(tabTodelete);
                }
            }
        }

        protected virtual async Task<bool> CanNavigateAsync(TabItem fromTab, TabItem toTab, NavigationContext context)
        {
            ITabSwitchAsyncAware removedTabAware = GetSwitchAsyncAware(fromTab);
            ITabSwitchAsyncAware addedTabAware = GetSwitchAsyncAware(toTab);
            bool canNavigate = removedTabAware == null || await removedTabAware.CanSwitchFrom(context);
            canNavigate = canNavigate && (addedTabAware == null || await addedTabAware.CanSwitchTo(context));
            return canNavigate;
        }

        protected virtual TabItem CreateTabItem(UserControl control)
        {
            TabItem item = new TabItem {
                Header = control.Name,
                Content = control,
                Width = 100,
                Height = 48,
                Padding = new Thickness(2)
            };
            item.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignNavigationRailTabItem");
            return item;
        }

        protected override IRegion CreateRegion() => new SingleActiveRegion();

        private ITabSwitchAsyncAware GetSwitchAsyncAware(TabItem item)
            => item?.Content is FrameworkElement el ? el.DataContext as ITabSwitchAsyncAware : null;
    }
}
