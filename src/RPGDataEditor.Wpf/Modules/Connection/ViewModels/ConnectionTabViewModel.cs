using FluentValidation.Results;
using RPGDataEditor.Connection;
using RPGDataEditor.Core.Commands;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace RPGDataEditor.Wpf.Connection.ViewModels
{
    public class ConnectionTabViewModel : ScreenViewModel, IValidationHook
    {
        public ConnectionTabViewModel(ViewModelContext context) : base(context) { }

        private IConnectionSettings connectionSettings;
        public IConnectionSettings ConnectionSettings {
            get => connectionSettings;
            set => SetProperty(ref connectionSettings, value);
        }

        public event EventHandler<ValidatedEventArgs> Validated;

        protected void RaiseValidated(object instance, ValidationResult result) => Validated?.Invoke(this, new ValidatedEventArgs(instance, result));

        public override Task<bool> CanNavigateTo(INavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanNavigateFrom(INavigationContext navigationContext)
        {
            ValidationResult result = await Context.Mediator.Send(new ValidateResourceQuery(ConnectionSettings, typeof(IConnectionSettings)));
            RaiseValidated(ConnectionSettings, result);
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
                catch (Exception ex)
                {
                    Context.Logger.Error("Couldn't save session", ex);
                }
            }
            return result.IsValid;
        }

        public override Task OnNavigatedToAsync(INavigationContext navigationContext)
        {
            ConnectionSettings = new ConnectionSettings(Context.Connection.Config);
            Context.Connection.Checker.Stop();
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(INavigationContext navigationContext)
        {
            Context.Connection.Configure(ConnectionSettings.CreateConfig());
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
