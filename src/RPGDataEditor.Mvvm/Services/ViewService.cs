namespace RPGDataEditor.Mvvm.Services
{
    public class ViewService : IViewService
    {
        public ViewService(IDialogService dialogService, ISnackbarService snackbarService)
        {
            DialogService = dialogService;
            SnackbarService = snackbarService;
        }

        public IDialogService DialogService { get; }

        public ISnackbarService SnackbarService { get; }
    }
}
