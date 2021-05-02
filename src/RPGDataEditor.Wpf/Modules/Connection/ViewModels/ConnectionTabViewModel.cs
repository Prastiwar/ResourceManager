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
        public ConnectionTabViewModel(ViewModelContext context, IConnectionChecker connectionChecker)
            : base(context) => ConnectionChecker = connectionChecker;

        protected IConnectionChecker ConnectionChecker { get; }

        public override Task<bool> CanNavigateTo(INavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanNavigateFrom(INavigationContext navigationContext)
        {
            ValidationResult result = await Context.Mediator.Send(new ValidateResourceQuery<IConnectionSettings>(Context.Connection));
            if (result.IsValid)
            {
                bool connected = await ConnectionChecker.ForceCheckAsync(default);
                if (!connected)
                {
                    return false;
                }
                try
                {
                    Session.SaveSession();
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
            ConnectionChecker.Stop();
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(INavigationContext navigationContext)
        {
            ConnectionChecker.Start();
            ConnectionChecker.Changed -= ConnectionChecker_Changed;
            ConnectionChecker.Changed += ConnectionChecker_Changed;
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
