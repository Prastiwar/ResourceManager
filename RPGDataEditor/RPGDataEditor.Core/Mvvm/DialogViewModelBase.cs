using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class DialogViewModelBase : ViewModelBase, IDialogAware
    {
        public DialogViewModelBase(ViewModelContext context) : base(context) { }

        public virtual string Title => "Dialog";

        public event Action<IDialogResult> RequestClose;

        private DelegateCommand<object> closeDialogCommand;
        public DelegateCommand<object> CloseDialogCommand => closeDialogCommand ??= new DelegateCommand<object>(CloseDialog);

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
            bool isSuccess = parameters.GetValue<bool>(nameof(DialogParametersBuilder.IsSuccess));
            ButtonResult result = isSuccess ? ButtonResult.OK : ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result, parameters));
        }

        public virtual async void RaiseRequestClose(IDialogResult dialogResult)
        {
            await WaitUntilRequestIsNotNull();
            RequestClose?.Invoke(dialogResult);
        }

        protected Task WaitUntilRequestIsNotNull()
        {
            while (RequestClose == null)
            {
                Task.Delay(100);
            }
            return Task.CompletedTask;
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
                RaiseRequestClose(new DialogResult(ButtonResult.Abort, new DialogParametersBuilder() { Exception = ex }.Build()));
            }
        }

        protected abstract Task InitializeAsync(IDialogParameters parameters);
    }
}
