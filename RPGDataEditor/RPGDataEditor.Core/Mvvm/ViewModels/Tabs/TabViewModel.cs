using Prism.Regions;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public class TabViewModel : ViewModelBase, ITabSwitchAsyncAware
    {
        public TabViewModel(ViewModelContext context) : base(context) { }

        private bool isLoading;
        public bool IsLoading {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public virtual Task OnNavigatedFromAsync(NavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task OnNavigatedToAsync(NavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task<bool> CanSwitchTo(NavigationContext navigationContext) => Session.Client.ConnectAsync();

        public virtual Task<bool> CanSwitchFrom(NavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task Refresh() => Task.CompletedTask;
    }
}
