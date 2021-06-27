namespace ResourceManager.Mvvm.Services
{
    public interface IViewService
    {
        IDialogService DialogService { get; }

        ISnackbarService SnackbarService { get; }
    }
}
