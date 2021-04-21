using Prism.Regions;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public interface ITabSwitchAsyncAware
    {
        Task OnNavigatedFromAsync(NavigationContext navigationContext);

        Task OnNavigatedToAsync(NavigationContext navigationContext);

        Task<bool> CanSwitchTo(NavigationContext navigationContext);

        Task<bool> CanSwitchFrom(NavigationContext navigationContext);
    }
}
