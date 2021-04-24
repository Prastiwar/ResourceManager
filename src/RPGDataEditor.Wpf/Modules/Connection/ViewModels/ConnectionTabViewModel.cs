using FluentValidation.Results;
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

        //public override async Task<bool> CanNavigateFrom(INavigationContext navigationContext)
        //{
        //    ValidationResult result = await Context.ValidationProvider.ValidateAsync(Session);
        //    if (result.IsValid)
        //    {
        //        bool connected = await Session.Client.ConnectAsync();
        //        if (!connected)
        //        {
        //            return false;
        //        }
        //        try
        //        {
        //            Session.SaveSession();
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Context.Logger.Error("Couldn't save session", ex);
        //        }
        //    }
        //    return result.IsValid;
        //}

        public override async Task OnNavigatedToAsync(INavigationContext navigationContext)
        {
            //await Session.Client.DisconnectAsync();
            //Context.ConnectionService.StopChecking();
        }

        public override async Task OnNavigatedFromAsync(INavigationContext navigationContext)
        {
            //await Session.Client.ConnectAsync();
            //Context.ConnectionService.StartChecking();
            //Context.ConnectionService.ConnectionChanged -= ConnectionChecker_Changed;
            //Context.ConnectionService.ConnectionChanged += ConnectionChecker_Changed;
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
