using Prism.Regions;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public class TabViewModel : ViewModelBase, ITabSwitchAsyncAware
    {
        public TabViewModel(SessionContext context) : base(context) { }

        public virtual Task OnNavigatedFromAsync(NavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task OnNavigatedToAsync(NavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task<bool> CanSwitchTo(NavigationContext navigationContext) => Context.IsValidAsync();

        public virtual Task<bool> CanSwitchFrom(NavigationContext navigationContext) => Task.FromResult(true);
    }
}
