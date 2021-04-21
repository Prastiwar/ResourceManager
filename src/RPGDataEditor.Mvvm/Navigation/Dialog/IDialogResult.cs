namespace RPGDataEditor.Mvvm.Navigation
{
    public interface IDialogResult
    {
        bool IsSuccess { get; }

        IDialogParameters Parameters { get; }
    }
}
