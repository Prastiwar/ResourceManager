using FluentValidation.Results;
using RPGDataEditor.Connection;
using RPGDataEditor.Core.Commands;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Navigation;
using System.Threading.Tasks;
using System.Windows;

namespace RPGDataEditor.Wpf.Connection.ViewModels
{
    public class ConnectionTabViewModel : ScreenViewModel
    {
        public ConnectionTabViewModel(ViewModelContext context) : base(context) { }

        public override Task<bool> CanNavigateTo(INavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanNavigateFrom(INavigationContext navigationContext)
        {
            ValidationResult result = await Context.Mediator.Send(new ValidateResourceQuery<IConnectionConfig>(Context.Connection.Config));
            if (result.IsValid)
            {
                bool connected = await Context.Connection.Checker.ForceCheckAsync(default);
                if (!connected)
                {
                    return false;
                }
                try
                {
                    await Context.Persistance.SaveConfigAsync(Context.Connection.Config);
                }
                catch (System.Exception ex)
                {
                    Context.Logger.Error("Couldn't save session", ex);
                }
            }
            return result.IsValid;
        }

        public override Task OnNavigatedToAsync(INavigationContext navigationContext)
        {
            Context.Connection.Checker.Stop();
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(INavigationContext navigationContext)
        {
            Context.Connection.Checker.Start();
            Context.Connection.Checker.Changed -= ConnectionChecker_Changed;
            Context.Connection.Checker.Changed += ConnectionChecker_Changed;
            return Task.CompletedTask;
        }

        private async void ConnectionChecker_Changed(object sender, bool hasConnection)
        {
            if (!hasConnection)
            {
                await Application.Current.Dispatcher.Invoke(async () => await Context.DialogService.ShowDialogAsync(DialogNames.ConnectionDialog));
            }
        }
    }
}
