namespace RPGDataEditor.Mvvm.Services
{
    public interface IViewService
    {
        IDialogService DialogService { get; }

        ISnackbarService SnackbarService { get; }
    }
}
