using ResourceManager.Mvvm.Services;

namespace ResourceManager.Wpf.ViewModels
{
    public class MainWindowViewModel : AppWindowViewModel
    {
        public MainWindowViewModel(ISnackbarService snackbarService) => SnackbarService = snackbarService;

        public ISnackbarService SnackbarService { get; }
    }
}
