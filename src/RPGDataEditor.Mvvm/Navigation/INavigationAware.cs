using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm.Navigation
{
    public interface INavigationAware
    {
        Task OnNavigatedFromAsync(INavigationContext navigationContext);

        Task OnNavigatedToAsync(INavigationContext navigationContext);

        Task<bool> CanNavigateTo(INavigationContext navigationContext);

        Task<bool> CanNavigateFrom(INavigationContext navigationContext);
    }
}
