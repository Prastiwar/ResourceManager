using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class MainWindowViewModel : AppWindowViewModel
    {
        public MainWindowViewModel(ISnackbarService snackbarService) => SnackbarService = snackbarService;

        public ISnackbarService SnackbarService { get; }
    }
}
