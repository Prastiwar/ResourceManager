using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Services.Dialogs;
using ResourceManager.Extensions.Prism.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResourceManager.Mvvm
{
    public abstract class DialogViewModelBase : BindableClass, IDialogAware
    {
        public DialogViewModelBase(ILogger<DialogViewModelBase> logger) => Logger = logger;

        public virtual string Title => "Dialog";

        public event Action<IDialogResult> RequestClose;

        private ICommand closeDialogCommand;
        public ICommand CloseDialogCommand => closeDialogCommand ??= new DelegateCommand<object>(CloseDialog);

        protected ILogger<DialogViewModelBase> Logger { get; }

        protected virtual void CloseDialog(object parameter)
        {
            ButtonResult result = ButtonResult.None;
            if (parameter is bool b)
            {
                result = b ? ButtonResult.OK : ButtonResult.Cancel;
            }
            RaiseRequestClose(new DialogResult(result));
        }

        public void Close(IDialogParameters parameters)
        {
            bool isSuccess = parameters.GetValue<bool>(nameof(Navigation.DialogParametersBuilder.IsSuccess));
            ButtonResult result = isSuccess ? ButtonResult.OK : ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result, parameters));
        }

        public virtual async void RaiseRequestClose(IDialogResult dialogResult)
        {
            await WaitUntilRequestIsNotNull();
            RequestClose?.Invoke(dialogResult);
        }

        protected async Task WaitUntilRequestIsNotNull()
        {
            while (RequestClose == null)
            {
                await Task.Delay(100);
            }
        }

        public virtual bool CanCloseDialog() => true;

        public virtual void OnDialogClosed() { }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                await InitializeAsync(parameters);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't initialize dialog");
                RaiseRequestClose(new DialogResult(ButtonResult.Abort, new Navigation.DialogParametersBuilder() { Exception = ex }.BuildPrism()));
            }
        }

        protected abstract Task InitializeAsync(IDialogParameters parameters);
    }
}
