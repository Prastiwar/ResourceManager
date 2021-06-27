namespace ResourceManager.Mvvm.Navigation
{
    public class DialogResult : IDialogResult
    {
        public DialogResult(bool isSuccess, IDialogParameters parameters = null)
        {
            IsSuccess = isSuccess;
            Parameters = parameters;
        }

        public bool IsSuccess { get; }

        public IDialogParameters Parameters { get; }
    }
}
